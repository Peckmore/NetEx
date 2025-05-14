using NetEx.Hooks.Interop;
using System.Runtime.InteropServices;
using System.Threading;

#if NET462_OR_GREATER || NETCOREAPP
using System.Threading.Tasks;
#endif

namespace NetEx.Hooks
{
    /// <summary>
    /// A keyboard event simulator, which can simulate <c>KeyDown</c>, <c>KeyUp</c>, and <c>KeyPress</c> events.
    /// </summary>
    public static class KeyboardSimulator
    {
        #region Methods

        #region Private

        private static bool SendKey(Keys key, bool keyUp)
        {
            key = key switch
            {
                Keys.Modifiers => Keys.None,
                Keys.Shift => Keys.ShiftKey,
                Keys.Control => Keys.ControlKey,
                Keys.Alt => Keys.Menu,
                _ => key
            };

            var input = new INPUT(NativeMethods.INPUT_KEYBOARD);
            input.input.ki.wScan = 0;
            input.input.ki.time = 0;
            input.input.ki.dwFlags = 0;
            input.input.ki.wVk = (ushort)key;

            if (keyUp)
            {
                input.input.ki.dwFlags = NativeMethods.KEYEVENTF_KEYUP;
            }

            return NativeMethods.SendInput(input, Marshal.SizeOf(input)) == 1;
        }

        #endregion

        #region Public

        /// <summary>
        /// Simulates a <c>KeyDown</c> event..
        /// </summary>
        /// <param name="key">The key to simulate.</param>
        /// <returns><see langword="true"/> if the event was sent successfully; otherwise <see landword="false"/></returns>
        public static bool KeyDown(Keys key)
        {
            return SendKey(key, false);
        }
        /// <summary>
        /// Simulates a <c>KeyPress</c> event..
        /// </summary>
        /// <param name="key">The key to simulate.</param>
        /// <returns><see langword="true"/> if the event was sent successfully; otherwise <see landword="false"/></returns>
        /// <remarks>A <c>KeyPress</c> event consists of a <c>KeyDown</c> event, followed by a short delay, then a <c>KeyUp</c> event.</remarks>
        public static bool KeyPress(Keys key)
        {
            if (KeyDown(key))
            {
                KeyDown(key);
                Thread.Sleep(10);
                return KeyUp(key);
            }

            return false;
        }
#if NET462_OR_GREATER || NETCOREAPP
        /// <summary>
        /// Simulates a <c>KeyPress</c> event..
        /// </summary>
        /// <param name="key">The key to simulate.</param>
        /// <returns><see langword="true"/> if both events were sent successfully; otherwise <see landword="false"/></returns>
        /// <remarks>
        /// <para>A <c>KeyPress</c> event consists of a <c>KeyDown</c> event, followed by a short delay, then a <c>KeyUp</c> event.</para>
        /// <para>This method uses an awaitable <see cref="Task"/> to create the delay between <c>KeyDown</c> and <c>KeyUp</c> events.</para>
        /// </remarks>
        public static async Task<bool> KeyPressAsync(Keys key)
        {
            if (KeyDown(key))
            {
                await Task.Delay(10);
                return KeyUp(key);
            }

            return false;
        }
#endif
        /// <summary>
        /// Simulates a <c>KeyUp</c> event..
        /// </summary>
        /// <param name="key">The key to simulate.</param>
        /// <returns><see langword="true"/> if the event was sent successfully; otherwise <see landword="false"/></returns>
        public static bool KeyUp(Keys key)
        {
            return SendKey(key, true);
        }

        #endregion

        #endregion
    }
}