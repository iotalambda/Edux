import { defineConfig } from 'vite'
import cssInjectedByJsPlugin from "vite-plugin-css-injected-by-js";

// https://vitejs.dev/config/
export default defineConfig({
    define: {
        'process.env': {}
    },
    plugins: [
        cssInjectedByJsPlugin({
            jsAssetsFilterFunction: function customJsAssetsfilterFunction(
                outputChunk
            ) {
                return outputChunk.isEntry;
            },
        }),
    ],
    build: {
        emptyOutDir: true,
        outDir: "../../wwwroot/js/dist",
        lib: {
            entry: ["src/eduxjs.ts", "src/jsutils.ts"],
            formats: ["es"],
        },
    },
})
