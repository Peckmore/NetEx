using System;

namespace NetEx.Hooks
{
    internal delegate IntPtr LowLevelMouseProc(int nCode, int wParam, IntPtr lParam);
}