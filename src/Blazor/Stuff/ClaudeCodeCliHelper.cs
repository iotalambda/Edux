using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Edux.Blazor.Stuff;

public class ClaudeCodeCliHelper(IHostEnvironment environment) : ITransient
{
    const string SystemPrompt = """
        You are Edux, a highly skilled helper LLM who specializes in building and iterating Next.js web apps based on the user’s requirements. Always assume you have a Next.js app already created in your work folder and that `npm run dev` is running continuously for live updates.

        After making changes to files within the Next.js project, ALWAYS ENSURE THERE ARE NO COMPILE ERRORS BEFORE TELLING THE USER THAT YOU HAVE COMPLETED THE TASK! You can check them using the tooling under EduxMcp MCP server.
        """;
    readonly string systemPromptNoNewLines = SystemPrompt.ReplaceLineEndings("<br />");

    string? sessionId;

    public async Task Launch(CancellationToken ct)
    {
        var process = new Process
        {
            StartInfo = new()
            {
                FileName = "claude",
                Arguments = $"""
                    -p "hello! can you see my work folders?"
                    --mcp-config mcp-servers.json
                    --allowedTools "mcp__EduxMcp__DrainNextJSNpmRunDevConsoleOutputBuffer"
                    --system-prompt "{systemPromptNoNewLines}"
                    --dangerously-skip-permissions
                    --output-format json
                    """.ReplaceLineEndings(" "),
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
                // https://docs.anthropic.com/en/docs/claude-code/sdk#mcp-configuration
                Arguments = $"""
                    -p "{message}"
                    --resume "{sessionId}"
                    --mcp-config mcp-servers.json
                    --allowedTools "mcp__EduxMcp__DrainNextJSNpmRunDevConsoleOutputBuffer"
                    --system-prompt "{systemPromptNoNewLines}"
                    --dangerously-skip-permissions
                    --output-format json
                    """.ReplaceLineEndings(" "), // TODO: escape message
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
