namespace Edux.Blazor.Stuff;

public class LoadingHelper : IScoped, IDisposable
{
    int counter = 0;

    public Event OnLoadingChanged { get; } = new Event();

    public bool IsLoading => counter > 0;

    public void Increment()
    {
        Interlocked.Increment(ref counter);
        OnLoadingChanged.Invoke();
    }

    public void Decrement()
    {
        Interlocked.Decrement(ref counter);
        OnLoadingChanged.Invoke();
    }

    public void Dispose()
    {
        OnLoadingChanged.Dispose();
    }
}
