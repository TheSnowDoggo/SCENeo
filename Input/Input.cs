using SCENeo.Utils;

namespace SCENeo;

public static class Input
{
    private const int KeyHigher = 0x8000;

    public static bool KeyPressed(int vkCode)
    {
        return Convert.ToBoolean(WinApi.GetKeyState(vkCode) & KeyHigher);
    }

    public static bool KeyPressed(Key key)
    {
        return KeyPressed((int)key);
    }
}
