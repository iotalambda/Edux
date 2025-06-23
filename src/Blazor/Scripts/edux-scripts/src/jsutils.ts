import type { IJsUtilsAdapter } from "./typings";

async function blur() {
  if (document.activeElement && document.activeElement !== document.body) {
    (document.activeElement as any).blur();
  }
}

async function scrollToBottom(elementId: string) {
  const el = document.getElementById(elementId);
  if (el) {
    el.scrollTo({ behavior: "smooth", top: el.scrollHeight });
  }
}

export default { blur, scrollToBottom } as IJsUtilsAdapter;
