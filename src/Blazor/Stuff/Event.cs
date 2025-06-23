using System.Collections.Concurrent;

namespace Edux.Blazor.Stuff;

public class Event : IDisposable
{
    bool disposed = false;

    readonly ConcurrentDictionary<Action, bool> subscriptions = [];

    public void SubscribeIfNotDisposed(Action action)
    {
        if (disposed)
            return;
        subscriptions[action] = true;
    }

    public void TryUnsubscribeIfNotDisposed(Action action)
    {
        if (disposed)
            return;
        subscriptions.TryRemove(action, out _);
    }

    public void Invoke()
    {
        foreach (var action in subscriptions.Keys)
        {
            if (subscriptions.TryGetValue(action, out _))
            {
                try
                {
                    action();
                }
                catch { }
            }
        }
    }

    public void Dispose()
    {
        disposed = true;
        subscriptions.Clear();
    }
}
