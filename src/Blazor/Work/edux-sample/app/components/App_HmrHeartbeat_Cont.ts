"use client";

import { useEffect } from "react";

export default function HmrHeartbeat() {
  useEffect(() => {
    if (process.env.NODE_ENV !== "development") return;

    const OriginalWebSocket = window.WebSocket;
    /** @ts-ignore */
    window.WebSocket = class extends OriginalWebSocket {
      constructor(url: string, protocols?: string | string[]) {
        super(url, protocols);
        if (url.includes("/_next/webpack-hmr")) {
          this.addEventListener(
            "open",
            () => {
              const hb = setInterval(() => this.send('{ "type": "EDUX_HMRHEARTBEAT" }'), 30000);
              this.addEventListener("close", () => clearInterval(hb), { once: true });
            },
            { once: true }
          );
        }
      }
    };
  }, []);

  return null;
}
