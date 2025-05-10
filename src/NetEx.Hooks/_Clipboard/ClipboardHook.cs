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
    public class ClipboardHook
    {
        #region Variables

        private static int _messageLoopError;
        private static Thread? _messageLoopThread;
        private static IntPtr _nextWindowHandle;
        private static IntPtr _windowHandle;
        private static readonly Semaphore _windowSemaphore;
        private static WndProc? _wndProc;

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
            _windowSemaphore = new Semaphore(0, 1);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether <seealso cref="ClipboardHook"/> has been installed and is capturing clipboard events.
        /// </summary>
        /// <returns><see langword="true"/> if the hook is installed and valid; otherwise <see langword="false"/>.</returns>
        public static bool IsInstalled => _windowHandle != IntPtr.Zero;

        #endregion

        #region Methods

        #region Private Static

        private static void MessageLoop()
        {
            // Clear our last error value so we can track errors.
            _messageLoopError = 0;

            // Track whether we have released our semaphore
            var released = false;

            // Get the current process.
            using (var currentProcess = Process.GetCurrentProcess())
            {
                // Check that we can get a module name (which confirms that process, module, and name are not null).
                if (currentProcess.MainModule?.ModuleName != null)
                {
                    // Get the current module.
                    using (var currentModule = currentProcess.MainModule)
                    {
                        // Check whether we have registered our window class. We only want to do this once, but we'll only do it when the hook
                        // is installed.
                        if (_wndProc == null)
                        {
                            // Define the window class
                            _wndProc = WndProc;
                            var wndClass = new WNDCLASSEX(Marshal.GetFunctionPointerForDelegate(_wndProc), NativeMethods.GetModuleHandle(currentModule.ModuleName));

                            // Register the window class
                            if (NativeMethods.RegisterClassEx(ref wndClass) == 0)
                            {
                                // Registration failed, so we'll clean up and throw an exception to let the user know.
                                _wndProc = null;
                                throw new Win32Exception();
                            }
                        }

                        // Create a new window of our defined class type. We have to create a window as we need a message loop to receive window
                        // messages, which include clipboard updates.
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
                                                                     NativeMethods.GetModuleHandle(currentModule.ModuleName),
                                                                     IntPtr.Zero);
                    }
                }
            }

            // Check that the window was created successfully
            if (_windowHandle != IntPtr.Zero)
            {
                // As an extra check, make sure that our window is valid.
                var isValid = NativeMethods.IsWindow(_windowHandle);
                Debug.Assert(isValid);

                if (isValid)
                {
                    // Our window was created and is valid, so we can release our semaphore to let our calling method continue, and we
                    // can then start our message loop.
                    _windowSemaphore.Release();
                    released = true;

                    // Start our message loop. We call `GetMessage` to wait for a message, then process it once received. This will run
                    // until we receive a `WM_QUIT` message.
                    while (NativeMethods.GetMessage(out MSG msg, IntPtr.Zero, 0, 0))
                    {
                        // Translate and dispatch each message once received - `DispatchMessage` will call our WndProc method, which is
                        // where we can then identify and process the message.
                        NativeMethods.TranslateMessage(ref msg);
                        NativeMethods.DispatchMessage(ref msg);
                    }

                    // We've received a `WM_QUIT` message, so we can begin cleaning up.
                }

                // Destroy our window.
                if (!NativeMethods.DestroyWindow(_windowHandle))
                {
                    _messageLoopError = Marshal.GetLastWin32Error();
                }
            }
            else
            {
                _messageLoopError = Marshal.GetLastWin32Error();
            }

            if (!released)
            {
                _windowSemaphore.Release();
            }
        }
        private static IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case NativeMethods.WM_DRAWCLIPBOARD: // Indicates that the clipboard has updated.
                    // Raise our event...
                    ClipboardUpdated?.Invoke();

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
                    NativeMethods.ChangeClipboardChain(_windowHandle, _nextWindowHandle);
                    NativeMethods.PostQuitMessage(0);
                    break;
            }

            // Pass the message along to the default WndProc handler.
            return NativeMethods.DefWindowProc(hWnd, msg, wParam, lParam);
        }

        #endregion

        #region Public Static

        public static void Install()
        {
            if (_messageLoopThread == null)
            {
                // We need to create a thread to run our window message loop on, so we'll create the thread here and start it running.
                _messageLoopThread = new Thread(MessageLoop);
                _messageLoopThread.Start();

                // The thread will handle creating the window and then pumping the message queue, so we'll wait here until the thread
                // lets us know it has either initialized, or failed to initialize.
                _windowSemaphore.WaitOne();

                // The loop thread will have set `LastError` if an error occurred during initialization, so we'll check that now.
                if (_messageLoopError > 0)
                {
                    // There was an error, so let's throw it.
                    throw new Win32Exception(_messageLoopError);
                }

                // There was no error, so the thread should now be running and ready to handle messages. So now we can register our
                // window to receive clipboard messages. We have to do this after the message loop is running as registering to
                // receive clipboard messages is doing through windows messages.
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
        public static void Uninstall()
        {
            if (_messageLoopThread != null)
            {
                NativeMethods.PostDestroyMessage(_windowHandle);
                _messageLoopThread.Join();
                _messageLoopThread = null;
            }
        }
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

        #endregion

        #endregion
    }
}