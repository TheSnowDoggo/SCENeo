using System.Runtime.InteropServices;

namespace SCENeo.Utils;

internal static partial class WinApi
{
    private const string User32 = "user32.dll";

    [LibraryImport(User32)]
    internal static partial short GetKeyState(int nVirtKey);
}
