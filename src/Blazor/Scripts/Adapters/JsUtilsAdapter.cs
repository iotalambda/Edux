using Edux.Blazor.Stuff;
using Microsoft.JSInterop;
using Reinforced.Typings.Attributes;

namespace Edux.Blazor.Scripts.Adapters;

[TsInterface]
public class JsUtilsAdapter(IJSRuntime js, IWebHostEnvironment webHostEnvironment)
    : IScoped,
        IAsyncDisposable,
        IJsUtils
{
    IJSObjectReference? m;

    public async Task Blur([TsIgnore] CancellationToken ct)
    {
        var m = await Import(ct);
        await m.InvokeVoidAsync("default.blur", ct);
    }

    public async Task ScrollToBottom(string elementId, [TsIgnore] CancellationToken ct)
    {
        var m = await Import(ct);
        await m.InvokeVoidAsync("default.scrollToBottom", ct, [elementId]);
    }

    [TsIgnore]
    public async ValueTask DisposeAsync()
    {
        try
        {
            if (m is { })
            {
                await m.DisposeAsync();
            }
        }
        catch (ObjectDisposedException) { }
    }

    [TsIgnore]
    public async Task<IJSObjectReference> Import(CancellationToken ct)
    {
        return m ??= await js.ImportModule(webHostEnvironment, "./dist/js/jsutils.js", ct);
    }
}
