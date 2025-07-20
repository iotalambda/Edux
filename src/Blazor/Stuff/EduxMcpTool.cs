using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using ModelContextProtocol.Server;

namespace Edux.Blazor.Stuff;

[McpServerToolType]
public class EduxMcpTool
{
    [
        McpServerTool(
            Destructive = true,
            Idempotent = false,
            ReadOnly = false,
            Name = "DrainNextJSNpmRunDevConsoleOutputBuffer"
        ),
        Description(
            "Gets the pending Next.JS `npm run dev` stdout and stderr entries from the buffer and flushes it. You may use this to inspect compile errors after you have saved files within the Next.JS project."
        )
    ]
    public IEnumerable<string> DrainNextJSNpmRunDevConsoleOutputBuffer() =>
        NextJSConsoleOutputBuffer.Drain();

    [
        McpServerTool(
            Destructive = false,
            Idempotent = true,
            ReadOnly = true,
            Name = "ExecNpmRunLint"
        ),
        Description("Executes `npm run lint` and returns the stdout and stderr entries.")
    ]
    public IEnumerable<string> ExecNpmRunLint(
        [FromServices] NextJSNpmRunLintExecutor nextJSNpmRunLintExecutor
    ) => nextJSNpmRunLintExecutor.Execute();
}
