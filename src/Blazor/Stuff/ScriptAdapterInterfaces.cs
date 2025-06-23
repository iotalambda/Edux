namespace Edux.Blazor.Stuff;

public interface IJsUtils
{
    Task Blur(CancellationToken ct);
    Task ScrollToBottom(string elementId, CancellationToken ct);
}
