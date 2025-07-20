using System.Diagnostics;

namespace Edux.Blazor.Stuff;

public class NextJSNpmRunLintExecutor(IHostEnvironment environment) : ISingleton
{
    public IEnumerable<string> Execute()
    {
        var lint = new Process
        {
            StartInfo = new()
            {
                FileName = "npm",
                Arguments = "run lint",
                WorkingDirectory = Path.Combine(environment.ContentRootPath, "Work/edux-sample"),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            },
        };

        var result = new List<string>();
        lint.OutputDataReceived += (source, args) =>
        {
            result.Add($"stdout: {args.Data}");
            Console.WriteLine($"[EDUX] npm run lint OUT: {args.Data}");
        };
        lint.ErrorDataReceived += (source, args) =>
        {
            result.Add($"stderr: {args.Data}");
            Console.Error.WriteLine($"[EDUX] npm run lint ERR: {args.Data}");
        };

        lint.Start();
        lint.BeginOutputReadLine();
        lint.BeginErrorReadLine();
        lint.WaitForExit();

        return result;
    }
}
