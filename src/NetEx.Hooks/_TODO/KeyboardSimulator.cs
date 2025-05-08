using NetEx.Hooks.Internal;
using System.Runtime.InteropServices;
using System.Threading;

namespace NetEx.Hooks
{
    public static class KeyboardSimulator
    {
        #region Methods

        #region Private

        private static uint SendKey(Keys key, bool keyUp)
        {
            switch (key)
            {
                case Keys.Modifiers:
                    key = Keys.None;
                    break;
                case Keys.Shift:
                    key = Keys.ShiftKey;
                    break;
                case Keys.Control:
                    key = Keys.ControlKey;
                    break;
                case Keys.Alt:
                    key = Keys.Menu;
                    break;
            }

            INPUT input = new INPUT(NativeMethods.INPUT_KEYBOARD);
            input.input.ki.wScan = 0;
            input.input.ki.time = 0;
            input.input.ki.dwFlags = 0;
            input.input.ki.wVk = (ushort)key;

            if (keyUp)
            {
                input.input.ki.dwFlags = NativeMethods.KEYEVENTF_KEYUP;
            }

            return NativeMethods.SendInput(input, Marshal.SizeOf(input));
        }

        #endregion

        #region Public

        public static void KeyDown(Keys Key)
        {
            SendKey(Key, false);
        }
        public static void KeyPress(Keys Key)
        {
            KeyDown(Key);
            Thread.Sleep(10);
            KeyUp(Key);
        }
        public static void KeyUp(Keys Key)
        {
            SendKey(Key, true);
        }

        #endregion

        #endregion
    }
}