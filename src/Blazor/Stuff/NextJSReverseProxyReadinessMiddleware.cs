namespace Edux.Blazor.Stuff;

public class NextJSReverseProxyReadinessMiddleware : IMiddleware, ISingleton
{
    int readinessState;
    readonly TaskCompletionSource readyTcs = new();

    public Task Ready => readyTcs.Task;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (readinessState == 0 && Interlocked.CompareExchange(ref readinessState, 1, 0) == 0)
        {
            var downstreamAddress = context
                .GetReverseProxyFeature()
                .AvailableDestinations[0]
                .Model.Config.Address;
            downstreamAddress = $"{downstreamAddress}d";
            var attempt = 0;
            const int attemptsMax = 10;
            while (true)
            {
                attempt++;
                try
                {
                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                    var httpClient = context
                        .RequestServices.GetRequiredService<IHttpClientFactory>()
                        .CreateClient();
                    var res = await httpClient.GetAsync(downstreamAddress, cts.Token);
                    res.EnsureSuccessStatusCode();
                    readyTcs.SetResult();
                    readinessState = 2;
                    break;
                }
                catch when (attempt < attemptsMax)
                {
                    await Task.Delay(TimeSpan.FromSeconds(2));
                }
                catch (Exception e)
                {
                    readyTcs.SetException(e);
                    throw;
                }
            }
        }
        else if (readinessState == 1)
        {
            await readyTcs.Task;
        }

        await next(context);
    }
}
