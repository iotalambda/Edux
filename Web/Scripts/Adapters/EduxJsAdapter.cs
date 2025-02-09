using Microsoft.JSInterop;
using Reinforced.Typings.Attributes;

namespace Edux.Web.Scripts.Adapters;

[TsInterface]
public class EduxJsAdapter(IJSRuntime js, IWebHostEnvironment webHostEnvironment) : IScoped, IAsyncDisposable, IEduxJs
{
    IJSObjectReference? m;

    public async Task HandleEduxEvent(string eduxEventKey, [TsIgnore] CancellationToken ct)
    {
        var m = await Import(ct);
        await m.InvokeVoidAsync(Utils.ResolveJsMethodName(nameof(HandleEduxEvent)), ct, [eduxEventKey]);
    }

    public async Task SetEventHandlerJs(string value, [TsIgnore] CancellationToken ct)
    {
        var m = await Import(ct);
        await m.InvokeVoidAsync(Utils.ResolveJsMethodName(nameof(SetEventHandlerJs)), ct, [value]);
    }

    public async Task ResetEduxRoot([TsIgnore] CancellationToken ct)
    {
        var m = await Import(ct);
        await m.InvokeVoidAsync(Utils.ResolveJsMethodName(nameof(ResetEduxRoot)), ct);
    }

    [TsIgnore]
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (m is { })
                await m.DisposeAsync();
        }
        catch (ObjectDisposedException) { }
    }

    [TsIgnore]
    public async Task<IJSObjectReference> Import(CancellationToken ct)
    {
        return m ??= await js.ImportModule(webHostEnvironment, "./js/dist/eduxjs.js", ct);
    }
}
