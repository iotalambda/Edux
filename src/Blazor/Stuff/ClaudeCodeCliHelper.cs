using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Edux.Blazor.Stuff;

public class ClaudeCodeCliHelper(IHostEnvironment environment) : ITransient
{
    string? sessionId;

    public async Task Launch(CancellationToken ct)
    {
        var process = new Process
        {
            StartInfo = new()
            {
                FileName = "claude",
                Arguments = "-p \"hello! can you see my work folders?\" --output-format json",
                WorkingDirectory = Path.Combine(environment.ContentRootPath, "Work/edux-sample"),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            },
        };
        ClaudeCodeCliPromptResponse? promptResponse = null;
        process.OutputDataReceived += (source, args) =>
        {
            if (args?.Data == null)
            {
                return;
            }

            Console.WriteLine($"[EDUX] Claude {nameof(Launch)} OUT: {args.Data}");

            if (args.Data is ['{', ..] data)
            {
                promptResponse = JsonSerializer.Deserialize<ClaudeCodeCliPromptResponse>(data);
            }
        };
        process.ErrorDataReceived += (source, args) =>
        {
            if (args?.Data == null)
            {
                return;
            }

            Console.Error.WriteLine($"[EDUX] Claude {nameof(Launch)} ERR: {args.Data}");
        };
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.StandardInput.Close();
        await process.WaitForExitAsync(ct);

        sessionId = promptResponse?.SessionId;
    }

    public async Task<string?> Prompt(string message, CancellationToken ct)
    {
        var process = new Process
        {
            StartInfo = new()
            {
                FileName = "claude",
                Arguments = $"-p \"{message}\" --resume \"{sessionId}\" --output-format json", // TODO: escape message
                WorkingDirectory = Path.Combine(environment.ContentRootPath, "Work/edux-sample"),
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            },
        };
        ClaudeCodeCliPromptResponse? promptResponse = null;
        process.OutputDataReceived += (source, args) =>
        {
            if (args?.Data == null)
            {
                return;
            }

            Console.WriteLine($"[EDUX] Claude {nameof(Prompt)} OUT: {args.Data}");

            if (args.Data is ['{', ..] data)
            {
                promptResponse = JsonSerializer.Deserialize<ClaudeCodeCliPromptResponse>(data);
            }
        };
        process.ErrorDataReceived += (source, args) =>
        {
            if (args?.Data == null)
            {
                return;
            }

            Console.Error.WriteLine($"[EDUX] Claude {nameof(Prompt)} ERR: {args.Data}");
        };
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.StandardInput.Close();
        await process.WaitForExitAsync(ct);

        sessionId = promptResponse?.SessionId;
        return promptResponse?.Result;
    }
}

public class ClaudeCodeCliPromptResponse
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("subtype")]
    public string? Subtype { get; set; }

    [JsonPropertyName("is_error")]
    public bool? IsError { get; set; }

    [JsonPropertyName("duration_ms")]
    public int? DurationMs { get; set; }

    [JsonPropertyName("duration_api_ms")]
    public int? DurationApiMs { get; set; }

    [JsonPropertyName("num_turns")]
    public int? NumTurns { get; set; }

    [JsonPropertyName("result")]
    public string? Result { get; set; }

    [JsonPropertyName("session_id")]
    public string? SessionId { get; set; }

    [JsonPropertyName("total_cost_usd")]
    public double? TotalCostUsd { get; set; }

    [JsonPropertyName("usage")]
    public ClaudeCodeCliPromptResponseUsage? Usage { get; set; }
}

public class ClaudeCodeCliPromptResponseUsage
{
    [JsonPropertyName("input_tokens")]
    public int? InputTokens { get; set; }

    [JsonPropertyName("cache_creation_input_tokens")]
    public int? CacheCreationInputTokens { get; set; }

    [JsonPropertyName("cache_read_input_tokens")]
    public int? CacheReadInputTokens { get; set; }

    [JsonPropertyName("output_tokens")]
    public int? OutputTokens { get; set; }

    [JsonPropertyName("server_tool_use")]
    public ClaudeCodeCliPromptResponseServerToolUse? ServerToolUse { get; set; }
}

public class ClaudeCodeCliPromptResponseServerToolUse
{
    [JsonPropertyName("web_search_requests")]
    public int? WebSearchRequests { get; set; }
}
