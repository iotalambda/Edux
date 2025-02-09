import { IEduxJsAdapter } from "./typings";

let eventHandlerJs: string = "{}";

function handleEduxEvent(eduxEventKey: string) {
    eval(eventHandlerJs);
}

function setEventHandlerJs(value: string) {
    eventHandlerJs = value;
}

function resetEduxRoot() {
    document.getElementById('edux-root')?.remove();
    const container = document.getElementById('edux-root-container');
    if (!container) {
        console.error('EDUX: Element edux-root-container not found!');
        return;
    }
    const root = document.createElement('div');
    root.id = "edux-root";
    container.appendChild(root);
}

export default { handleEduxEvent, setEventHandlerJs, resetEduxRoot } as IEduxJsAdapter