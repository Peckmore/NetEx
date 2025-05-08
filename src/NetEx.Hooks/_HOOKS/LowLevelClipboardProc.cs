using NetEx.Hooks;
using System;
using System.Diagnostics;

namespace NetEx.Windows.Forms
{
    [DebuggerNonUserCode]
    internal class LowLevelClipboardProc : NativeWindow
    {

        #region Variables

        private IntPtr _nextWindowHandle;

        #endregion

        #region Events

        public event EventHandler WindowClosing;
        public event EventHandler ClipboardChanged;

        #endregion

        #region Properties

        public IntPtr NextWindow
        {
            get
            {
                return _nextWindowHandle;
            }
            set
            {
                _nextWindowHandle = value;
            }
        }

        #endregion

        #region Constructor

        public LowLevelClipboardProc(IntPtr NextWindowHandle)
        {
            _nextWindowHandle = NextWindowHandle;
        }

        #endregion

        #region Methods

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case NativeMethods.WM_DRAWCLIPBOARD:
                    if (ClipboardChanged != null)
                    {
                        ClipboardChanged(this, EventArgs.Empty);
                    }

                    NativeMethods.SendMessage(_nextWindowHandle, (uint)m.Msg, m.WParam, m.LParam);
                    break;
                case NativeMethods.WM_CHANGECBCHAIN:
                    if (m.WParam == _nextWindowHandle)
                    {
                        _nextWindowHandle = m.LParam;
                    }
                    else
                    {
                        NativeMethods.SendMessage(_nextWindowHandle, (uint)m.Msg, m.WParam, m.LParam);
                    }

                    break;
                default:
                    if (m.Msg == NativeMethods.WM_DESTROY && WindowClosing != null)
                    {
                        WindowClosing(this, new EventArgs());
                    }

                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion

    }
}