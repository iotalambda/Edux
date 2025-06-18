using System.Diagnostics;

namespace Edux.Blazor.Stuff;

public class ViteRunnerHostedService(IHostEnvironment environment) : IHostedService, IDisposable
{
    Process? vite = null;

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

        vite = new Process
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

        vite.OutputDataReceived += (source, args) =>
        {
            Console.WriteLine($"[EDUX] Vite OUT: {args.Data}");
        };

        vite.ErrorDataReceived += (source, args) =>
        {
            Console.WriteLine($"[EDUX] Vite ERR: {args.Data}");
        };

        vite.Start();
        vite.BeginOutputReadLine();
        vite.BeginErrorReadLine();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (vite is not { })
        {
            return Task.CompletedTask;
        }

        vite.Kill(true);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        if (vite is not { })
        {
            return;
        }

        vite.Kill(true);
    }
}
