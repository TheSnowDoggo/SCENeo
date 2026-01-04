namespace SCENeo.Testing;

internal static class Program
{
    private static void Main()
    {
        Console.CursorVisible = false;

        var manager = new ManagerUI2();

        manager.Run();
    }
}