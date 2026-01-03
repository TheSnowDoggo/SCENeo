using SCENeo.Ui;

namespace SCENeo.Testing;

internal static class Program
{
    private static void Main()
    {
        Console.CursorVisible = false;

        var manager = new ManagerUi();

        manager.Run();
    }
}