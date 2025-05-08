using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using NetEx.Hooks;

namespace NetEx.Windows.Forms
{
    public class ClipboardEx
    {

        #region Variables

        private static ClipboardUpdatedEventHandler _clipboardChangedEventHandler;
        private static LowLevelClipboardProc _lowLevelClipboardProc;
        private static IntPtr _windowHandle;

        #endregion

        #region Events

        public static event ClipboardUpdatedEventHandler ClipboardChanged
        {
            add
            {
                if (!Active)
                {
                    Hook();
                }

                _clipboardChangedEventHandler += value;
            }
            remove
            {
                _clipboardChangedEventHandler -= value;
                if (!HasEventHandlers && Active)
                {
                    Unhook();
                }
            }
        }

        #endregion

        #region Properties

        #region Private

        private static bool HasEventHandlers
        {
            get
            {
                return _clipboardChangedEventHandler != null;
            }
        }

        #endregion

        #region Public

        public static bool Active
        {
            get
            {
                return !(_lowLevelClipboardProc == null);
            }
        }

        #endregion

        #endregion

        #region Methods

        private static void Hook()
        {
            if (_lowLevelClipboardProc == null)
            {
                //_windowHandle = Application.OpenForms[0].Handle;
                IntPtr nextWindowHandle = NativeMethods.SetClipboardViewer(_windowHandle);
                int errorCode = Marshal.GetLastWin32Error();

                if (errorCode != 0)
                {
                    throw new Win32Exception(errorCode);
                }
                else
                {
                    _lowLevelClipboardProc = new LowLevelClipboardProc(nextWindowHandle);
                    _lowLevelClipboardProc.WindowClosing += new EventHandler(OnWindowClosing);
                    _lowLevelClipboardProc.ClipboardChanged += new EventHandler(OnClipboardChanged);
                    //_lowLevelClipboardProc.AssignHandle(_windowHandle);
#if DEBUG
                    Debug.WriteLine("Clipboard hooked");
#endif
                }

            }

        }
        private static void OnClipboardChanged(object sender, EventArgs e)
        {
            if (_clipboardChangedEventHandler != null)
            {
                //_clipboardChangedEventHandler(null, new ClipboardUpdatedEventArgs(Clipboard.GetDataObject().GetFormats()));
            }
        }
        private static void OnWindowClosing(object sender, EventArgs e)
        {
            Unhook();
        }
        private static void Unhook()
        {
            if (_lowLevelClipboardProc != null)
            {
                //NativeMethods.ChangeClipboardChain(_lowLevelClipboardProc.Handle, _lowLevelClipboardProc.NextWindow);
                int errorCode = Marshal.GetLastWin32Error();
                if (errorCode != 0)
                {
                    throw new Win32Exception(errorCode);
                }
                else
                {
                    //_lowLevelClipboardProc.ReleaseHandle();
                    _lowLevelClipboardProc.WindowClosing -= new EventHandler(OnWindowClosing);
                    _lowLevelClipboardProc.ClipboardChanged -= new EventHandler(OnClipboardChanged);
                    _lowLevelClipboardProc = null;
#if DEBUG
                    Debug.WriteLine("Clipboard unhooked");
#endif
                }
            }
        }

        #endregion

    }
}
