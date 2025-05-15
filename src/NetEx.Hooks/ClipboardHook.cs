using NetEx.Hooks.Interop;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace NetEx.Hooks
{
    /// <summary>
    /// Provides a mechanism for hooking all clipboard events within the operating system.
    /// </summary>
    /// <remarks>
    /// <para>There are three ways of monitoring changes to the clipboard. The oldest method is to create a clipboard viewer window. <b>Windows 2000</b> added the ability to query the clipboard sequence number, and <b>Windows Vista</b> added support for clipboard format listeners. Clipboard viewer windows are supported for backward compatibility with earlier versions of Windows.</para>
    /// <para>On <b>Windows Vista</b> and later, <c>ClipboardHook</c> will create a <c>Clipboard Format Listener</c> to listen for clipboard updates. On earlier versions it will create a <c>Clipboard Viewer Window</c> instead.</para>
    /// </remarks>
    public static class ClipboardHook
    {
        #region Fields

        private static readonly Semaphore _installationSemaphore;
        private static Thread? _messageLoopThread;
        private static int _messageLoopThreadError;
        private static IntPtr _nextWindowHandle;
        private static bool _useNewClipboardFormatListener;
        private static IntPtr _windowHandle;
        private static readonly Semaphore _windowSemaphore;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when data is placed onto the clipboard.
        /// </summary>
        public static event ClipboardUpdatedEventHandler? ClipboardUpdated;

        #endregion

        #region Construction

        static ClipboardHook()
        {
            // Initialize our semaphores
            _installationSemaphore = new(1, 1);
            _windowSemaphore = new(0, 1);

            // Check if we're on Windows Vista or later, and set our flag accordingly.
            if (Environment.OSVersion.Version >= new Version(6, 0))
            {
                // We're on Windows Vista or later, so we can use the newer "Clipboard Format Listener" API.
                _useNewClipboardFormatListener = true;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether <see cref="ClipboardHook"/> has been installed and is capturing clipboard events.
        /// </summary>
        /// <returns><see langword="true"/> if the hook is installed and valid; otherwise <see langword="false"/>.</returns>
        public static bool IsInstalled => _windowHandle != IntPtr.Zero;

        #endregion

        #region Methods

        #region Private Static

        private static void MessageLoop()
        {
            // Clear our last error value so we can track errors.
            _messageLoopThreadError = 0;

            // Track whether we have released our semaphore
            var released = false;

            // We're going to get the module handle, so create a variable to store it.
            var moduleHandle = IntPtr.Zero;

            // First, get the current process.
            using (var currentProcess = Process.GetCurrentProcess())
            {
                // Then check that we can get a module name (which confirms that process, module, and name are not null).
                if (currentProcess.MainModule?.ModuleName != null)
                {
                    // Get the current module.
                    using (var currentModule = currentProcess.MainModule)
                    {
                        // Finally, get the module handle.
                        moduleHandle = NativeMethods.GetModuleHandle(currentModule.ModuleName);
                    }
                }
            }

            // Check we have a valid module handle.
            if (moduleHandle != IntPtr.Zero)
            {
                // Define our window class
                WndProc wndProc = WndProc;
                var wndClass = new WNDCLASSEX(Marshal.GetFunctionPointerForDelegate(wndProc), moduleHandle);

                // Register the window class
                var atom = NativeMethods.RegisterClassEx(ref wndClass);
                if (atom > 0)
                {
                    // Create a new window of our defined class type. We have to create a window as we need a message loop to receive
                    // window messages, which include clipboard updates.
                    _windowHandle = NativeMethods.CreateWindowEx(0,
                                                                 WNDCLASSEX.ClassName,
                                                                 WNDCLASSEX.ClassName,
                                                                 0,
                                                                 0,
                                                                 0,
                                                                 0,
                                                                 0,
                                                                 IntPtr.Zero,
                                                                 IntPtr.Zero,
                                                                 moduleHandle,
                                                                 IntPtr.Zero);

                    // Check that the window was created successfully
                    if (_windowHandle != IntPtr.Zero)
                    {
                        // Our window was created and is valid, so we can release our semaphore to let our calling method continue, and
                        // we can then start our message loop.
                        _windowSemaphore.Release();
                        released = true;

                        // Start our message loop. We call `GetMessage` to wait for a message, then process it once received. This will
                        // run until we receive a `WM_QUIT` message.
                        while (NativeMethods.GetMessage(out MSG msg, IntPtr.Zero, 0, 0))
                        {
                            // Translate and dispatch each message once received - `DispatchMessage` will call our WndProc method, which
                            // is where we can then identify and handle the message.
                            NativeMethods.TranslateMessage(ref msg);
                            NativeMethods.DispatchMessage(ref msg);
                        }

                        // We've received a `WM_QUIT` message, so we can begin cleaning up.

                        if (_useNewClipboardFormatListener)
                        {
                            // We're on Windows Vista or later so we need to remove our clipboard format listener.
                            if (!NativeMethods.RemoveClipboardFormatListener(_windowHandle))
                            {
                                // We failed to remove the listener, so grab the error before we return.
                                _messageLoopThreadError = Marshal.GetLastWin32Error();
                            }
                        }

                        // Destroy our window. Although we've already sent a `WM_DESTROY` message we still need to call `DestroyWindow`
                        // to ensure that all resources are released.
                        if (!NativeMethods.DestroyWindow(_windowHandle))
                        {
                            // We failed to destroy the window, so grab the error before we return.
                            _messageLoopThreadError = Marshal.GetLastWin32Error();
                        }
                    }
                    else
                    {
                        // We failed to create the window, so grab the error before we return.
                        _messageLoopThreadError = Marshal.GetLastWin32Error();
                    }

                    // Unregister our window class.
                    if (!NativeMethods.UnregisterClass(WNDCLASSEX.ClassName, moduleHandle))
                    {
                        // We failed to unregister the window class, so grab the error before we return.
                        _messageLoopThreadError = Marshal.GetLastWin32Error();
                    }
                }
                else
                {
                    // We failed to register the window class, so grab the error before we return.
                    _messageLoopThreadError = Marshal.GetLastWin32Error();
                }

                // Ensure that our wndProc delegate is not GC'd until after we have finished with it.
                GC.KeepAlive(wndProc);
            }
            else
            {
                // We failed to get a valid module handle, so grab the error before we return.
                _messageLoopThreadError = Marshal.GetLastWin32Error();
            }

            // Finally, check whether the semaphore has already been released (due to successfully starting the message loop). If it
            // hasn't, we need to release it now.
            if (!released)
            {
                _windowSemaphore.Release();
            }
        }
        private static IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case NativeMethods.WM_CLIPBOARDUPDATE: // Clipboard content has changed.

                    // Raise our event.
                    ClipboardUpdated?.Invoke(new());
                    
                    break;

                case NativeMethods.WM_DRAWCLIPBOARD: // Clipboard content has changed.

                    // Raise our event...
                    ClipboardUpdated?.Invoke(new());

                    // ...then pass the message on to the next window.
                    NativeMethods.SendMessage(_nextWindowHandle, msg, wParam, lParam);
                    break;

                case NativeMethods.WM_CHANGECBCHAIN: // A clipboard handler is being removed.

                    // Check if we're the link in the chain that needs to update.
                    if (wParam == _nextWindowHandle)
                    {
                        // We are, so update our "next window" handle to the new handle.
                        _nextWindowHandle = lParam;
                    }
                    else
                    {
                        // We don't need to update, so pass the message on down the chain.
                        NativeMethods.SendMessage(_nextWindowHandle, msg, wParam, lParam);
                    }
                    break;

                case NativeMethods.WM_DESTROY: // Close our window.

                    if (!_useNewClipboardFormatListener)
                    {
                        // We're on a version of Windows earlier than Windows Vista, so we need to unhook our window from the clipboard chain.
                        NativeMethods.ChangeClipboardChain(_windowHandle, _nextWindowHandle);
                    }
                    
                    // Post a quit message to end the message loop.
                    NativeMethods.PostQuitMessage(0);
                    break;
            }

            // Pass the message along to the default WndProc handler.
            return NativeMethods.DefWindowProc(hWnd, msg, wParam, lParam);
        }

        #endregion

        #region Public Static

        /// <summary>
        /// Installs the clipboard hook, capturing all clipboard events.
        /// </summary>
        /// <exception cref="Win32Exception">The hook could not be installed.</exception>
        public static void Install()
        {
            // Check that an installation/uninstallation is not already in progress.
            _installationSemaphore.WaitOne();

            try
            {
                // Check we haven't already installed the hook.
                if (_messageLoopThread == null)
                {
                    // We need to create a thread to run our window message loop on, so we'll create the thread here and start it running.
                    _messageLoopThread = new Thread(MessageLoop);
                    _messageLoopThread.Start();

                    // The thread will handle creating the window and then pumping the message queue, so we'll wait here until the thread
                    // lets us know it has either initialized, or failed to initialize.
                    _windowSemaphore.WaitOne();

                    // The loop thread will have set `LastError` if an error occurred during initialization, so we'll check that now.
                    if (_messageLoopThreadError > 0)
                    {
                        // There was an error, so let's throw it.
                        throw new Win32Exception(_messageLoopThreadError);
                    }

                    // There was no error, so the thread should now be running and ready to handle messages. So now we can register our
                    // window to receive clipboard messages.
                    if (_useNewClipboardFormatListener)
                    {
                        // We are on Windows Vista or later, so we can use the newer "Clipboard Format Listener" API. This is much more
                        // efficient than the older "ClipboardViewer" API, and is the preferred method of monitoring clipboard changes.

                        // Register as a Clipboard Format Listener
                        if (!NativeMethods.AddClipboardFormatListener(_windowHandle))
                        {
                            // There was an error, so let's throw it.
                            throw new Win32Exception();
                        }
                    }
                    else
                    {
                        // We on a version of Windows earlier than Windows Vista, so we'll use the older "ClipboardViewer" API. We have to do
                        // this after the message loop is running as registering to receive clipboard messages is done through windows messages.
                        _nextWindowHandle = NativeMethods.SetClipboardViewer(_windowHandle);

                        // Check whether we received a handle to the next process in the clipboard chain. If we did (and the handle isn't 0)
                        // then we definitely know that we have successfully registered. However, if the handle is 0 it doesn't necessarily
                        // mean that we failed - it could be that there are no other processes in the clipboard chain. So if it is 0 we'll
                        // check the last error code to see if there was a problem.
                        if (_nextWindowHandle == IntPtr.Zero)
                        {
                            // Handle was 0, let's check last error.
                            var clipboardError = Marshal.GetLastWin32Error();
                            if (clipboardError > 0)
                            {
                                // There was an error, so let's throw it.
                                throw new Win32Exception(clipboardError);
                            }
                        }
                    }
                }
            }
            finally
            {
                // Release our semaphore to allow other threads to install/uninstall the hook.
                _installationSemaphore.Release();
            }
        }
        /// <summary>
        /// Attempts to install the clipboard hook.
        /// </summary>
        /// <returns><see langword="true"/> if the hook was successfully installed; otherwise <see langword="false"/>.</returns>
        public static bool TryInstall()
        {
            try
            {
                Install();
                return true;
            }
            catch (Exception e)
            {
                Debug.Assert(false, e.Message);
                return false;
            }
        }
        /// <summary>
        /// Attempts to uninstall the clipboard hook.
        /// </summary>
        /// <returns><see langword="true"/> if the hook was successfully installed; otherwise <see langword="false"/>.</returns>
        public static bool TryUninstall()
        {
            try
            {
                Uninstall();
                return true;
            }
            catch (Exception e)
            {
                Debug.Assert(false, e.Message);
                return false;
            }
        }
        /// <summary>
        /// Uninstalls the clipboard hook, and stops further clipboard events from being captured.
        /// </summary>
        /// <exception cref="Win32Exception">The hook could not be uninstalled.</exception>
        public static void Uninstall()
        {
            // Check that an installation/uninstallation is not already in progress.
            _installationSemaphore.WaitOne();

            try
            {
                // Check whether the hook has been installed before we attempt to uninstall it.
                if (_messageLoopThread != null)
                {
                    // Send a `WM_DESTORY` message to our window to close it down. This will also post a quit message to the message loop, which will
                    // cause it to exit.
                    NativeMethods.PostDestroyMessage(_windowHandle);

                    // Wait for the message loop thread to finish processing, then clear our reference to the thread.
                    _messageLoopThread.Join();
                    _messageLoopThread = null;
                }
            }
            finally
            {
                // Release our semaphore to allow other threads to install/uninstall the hook.
                _installationSemaphore.Release();
            }
        }

        #endregion

        #endregion
    }
}