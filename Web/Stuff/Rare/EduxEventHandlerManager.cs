namespace Edux.Web.Stuff.Rare;

public class EduxEventHandlerManager : IScoped
{
    readonly List<string> ifStatementsJs = [];

    public void AddIfStatementJs(string value) => ifStatementsJs.Add(value);

    public string GetEventHandlerJs() => string.Join(Environment.NewLine, ifStatementsJs);
}