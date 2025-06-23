namespace Edux.Blazor.Stuff;

public class EventSubscription : IDisposable
{
    private readonly Event ev;
    private readonly Action action;

    public EventSubscription(Event ev, Action action)
    {
        this.ev = ev;
        this.action = action;
        ev.SubscribeIfNotDisposed(action);
    }

    public void Dispose()
    {
        ev.TryUnsubscribeIfNotDisposed(action);
    }
}
