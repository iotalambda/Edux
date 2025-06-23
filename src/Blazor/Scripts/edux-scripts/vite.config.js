import { defineConfig } from "vite";

export default defineConfig({
  define: {
    "process.env": {},
  },
  build: {
    emptyOutDir: true,
    outDir: "../../wwwroot/dist/js",
    lib: {
      entry: ["src/jsutils.ts"],
      formats: ["es"],
    },
  },
});
