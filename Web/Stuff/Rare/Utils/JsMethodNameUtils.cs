namespace Edux.Web.Stuff.Rare.Utils;

public static class JsMethodNameUtils
{
    public static string ResolveJsMethodName(string csMethodName) => $"default.{csMethodName[0..1].ToLower()}{csMethodName[1..]}";
}
