using System.Collections.Concurrent;

namespace Edux.Web.Stuff;

public abstract class Notification
{
    readonly ConcurrentDictionary<object, Func<CancellationToken, Task>> subscriptions = [];

    public async Task Notify(CancellationToken ct)
    {
        var tasks = subscriptions.Values.Select(c => c(ct));
        await Task.WhenAll(tasks);
    }

    public void Subscribe(object subscriber, Func<CancellationToken, Task> callback)
    {
        subscriptions[subscriber] = callback;
    }

    public void Unsubscribe(object subscriber)
    {
        subscriptions.Remove(subscriber, out _);
    }
}
