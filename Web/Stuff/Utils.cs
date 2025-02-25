using System.Reflection;

namespace Edux.Web.Stuff;

public static class Utils
{
    public static async ValueTask DisposeAutoDisposables(object target)
    {
        var props = target.GetType().GetProperties();
        var fields = target.GetType().GetFields();
        IEnumerable<MemberInfo> members = [.. props, .. fields];
        List<Exception>? exceptions = null;

        foreach (var m in members)
        {
            if (m.GetCustomAttribute<AutoDisposableAttribute>() is not { })
                continue;

            object? value;
            try
            {
                value = m switch
                {
                    PropertyInfo p => p.GetValue(target),
                    FieldInfo f => f.GetValue(target),
                    _ => throw new Exception("EDUX: Member not PropertyInfo or FieldInfo.")
                };
            }
            catch (Exception e)
            {
                (exceptions ??= []).Add(e);
                continue;
            }

            try
            {
                if (value is IAsyncDisposable { } a)
                    await a.DisposeAsync();

                if (value is IDisposable { } d)
                    d.Dispose();
            }
            catch (ObjectDisposedException) { }
            catch (Exception e)
            {
                (exceptions ??= []).Add(e);
            }
        }

        if (exceptions is [_, ..])
            throw new AggregateException(exceptions);
    }
}
