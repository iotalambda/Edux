using System.ComponentModel;
using ModelContextProtocol.Server;

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

[McpServerToolType]
public static class EduxMcpTool
{
    [
        McpServerTool(
            Destructive = true,
            Idempotent = false,
            Name = "DrainNextJSNpmRunDevConsoleOutputBuffer"
        ),
        Description(
            "Gets the pending Next.JS `npm run dev` stdout and stderr entries from the buffer and flushes it. You may use this to inspect compile errors after you have saved files within the Next.JS project."
        )
    ]
    public static IEnumerable<string> DrainNextJSNpmRunDevConsoleOutputBuffer() =>
        NextJSConsoleOutputBuffer.Drain();
}
