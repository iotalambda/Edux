using Microsoft.SemanticKernel;
using System.ComponentModel;

namespace Edux.Web.Stuff.Rare;

public class EduxEventHandlerBuilderPlugin(EduxEventHandlerManager manager)
{
    [KernelFunction("add_individual_js_if_statement")]
    [Description("""
        This tool adds individual Javascript if statements to the EVENT HANDLER.

        The provided $value should be of format:
        ```js
        if (<CONDITION>) {
            <DOM_UPDATES>
        }
        ```
        where:
        * `<CONDITION>` is the condition when DOM updates should be done. For example, if the DOM updates should be done when the USER navigates to the page, then the condition should be `eduxEventKey === "PageLoad"`.
        * `<DOM_UPDATES>` are the DOM updates that should be done on the conditions. For example, if a `div` element with text "Hello!" should be added to the page, then the DOM updates should be:
          ```js
          const rootEl = document.getElementById("edux-root");
          const newEl = document.createElement("div");
          newEl.textContent = "Hello!";
          rootEl.appendChild(newEl);
          ```

        Statements are combined in the UI and the EVENT HANDLER is formed. All EVENTs are passed to the EVENT HANDLER.

        Following `eduxEventKey`s are supported:
        * `PageLoad` - when page loads up.
        """)]
    public void AddIfStatementJs(string value) => manager.AddIfStatementJs(value);
}