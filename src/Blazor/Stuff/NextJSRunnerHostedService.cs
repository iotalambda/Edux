using System.Diagnostics;

namespace Edux.Blazor.Stuff;

public class NextJSRunnerHostedService(IHostEnvironment environment) : IHostedService, IDisposable
{
    Process? nextjs = null;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var fuserKill = new Process
        {
            StartInfo = new()
            {
                FileName = "/bin/bash",
                Arguments = "-c \"fuser -k -n tcp 3000\"",
                UseShellExecute = false,
                CreateNoWindow = true,
            },
        };
        fuserKill.Start();
        fuserKill.WaitForExit();

        nextjs = new Process
        {
            StartInfo = new()
            {
                FileName = "npm",
                Arguments = "run dev",
                WorkingDirectory = Path.Combine(environment.ContentRootPath, "Work/edux-sample"),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            },
        };

        nextjs.OutputDataReceived += (source, args) =>
        {
            Console.WriteLine($"[EDUX] NextJS OUT: {args.Data}");
        };

        nextjs.ErrorDataReceived += (source, args) =>
        {
            if (args.Data?.Contains("EDUX_HMRHEARTBEAT") == true)
                return;

            Console.Error.WriteLine($"[EDUX] NextJS ERR: {args.Data}");
        };

        nextjs.Start();
        nextjs.BeginOutputReadLine();
        nextjs.BeginErrorReadLine();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (nextjs is not { })
        {
            return Task.CompletedTask;
        }

        nextjs.Kill(true);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        if (nextjs is not { })
        {
            return;
        }

        nextjs.Kill(true);
    }
}
