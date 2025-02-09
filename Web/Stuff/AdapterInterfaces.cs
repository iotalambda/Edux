namespace Edux.Web.Stuff;

public interface IEduxJs
{
    Task HandleEduxEvent(string eduxEventKey, CancellationToken ct);
    Task ResetEduxRoot(CancellationToken ct);
    Task SetEventHandlerJs(string value, CancellationToken ct);
}

public interface IJsUtils
{
    Task Blur(CancellationToken ct);
    Task ScrollToBottom(string elementId, CancellationToken ct);
}
