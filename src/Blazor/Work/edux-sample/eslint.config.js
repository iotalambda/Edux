import { FlatCompat } from "@eslint/eslintrc";
import js from "@eslint/js";
import globals from "globals";
import tseslint from "typescript-eslint";
import { globalIgnores } from "eslint/config";
import eduxNoHooksInPresentationFiles from "./eslint-plugins/eslint-plugin-no-hooks-in-presentation-files.js";
import eduxContainerFilesCannotBeTsx from "./eslint-plugins/eslint-plugin-container-files-cannot-be-tsx.js";
import eduxComponentConvention from "./eslint-plugins/eslint-plugin-edux-component-convention.js";

const compat = new FlatCompat({
  baseDirectory: import.meta.dirname,
});

export default tseslint.config([
  globalIgnores(["dist"]),
  ...compat.config({
    extends: ["next/core-web-vitals", "next/typescript"],
  }),
  {
    files: ["**/*.{ts,tsx}"],
    extends: [js.configs.recommended, tseslint.configs.recommended],
    languageOptions: {
      ecmaVersion: 2020,
      globals: globals.browser,
    },
    rules: {
      "@typescript-eslint/no-explicit-any": "off",
      "@typescript-eslint/ban-ts-comment": "off",
      "react-hooks/exhaustive-deps": "off",
      "react-hooks/no-children-prop": "off",
    },
  },
  {
    files: ["**/*_Cont.tsx"],
    plugins: {
      "edux-container-files-plugin": {
        rules: {
          "no-tsx": eduxContainerFilesCannotBeTsx,
        },
      },
    },
    rules: {
      "edux-container-files-plugin/no-tsx": "error",
    },
  },
  {
    files: ["**/*_Pres.tsx", "**/page.tsx", "**/layout.tsx"],
    plugins: {
      "edux-presentation-files-plugin": {
        rules: {
          "no-hooks": eduxNoHooksInPresentationFiles,
        },
      },
    },
    rules: {
      "edux-presentation-files-plugin/no-hooks": "error",
    },
  },
  {
    files: ["**/app/**"],
    plugins: {
      "edux-component-convention-plugin": {
        rules: {
          "component-convention": eduxComponentConvention,
        },
      },
    },
    rules: {
      "edux-component-convention-plugin/component-convention": "error",
    },
  },
]);
