using System;

namespace NetEx.Hooks
{
    internal delegate IntPtr LowLevelKeyboardProc(int nCode, int wParam, IntPtr lParam);
}