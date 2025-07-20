namespace Edux.Blazor.Stuff;

internal static class NextJSConsoleOutputBuffer
{
    static readonly Lock @lock = new();
    static List<string> buffer = [];

    public static void Push(string line)
    {
        using var _ = @lock.EnterScope();
        buffer.Add(line);
    }

    public static List<string> Drain()
    {
        using var _ = @lock.EnterScope();
        var result = buffer;
        buffer = [];
        return result;
    }
}
