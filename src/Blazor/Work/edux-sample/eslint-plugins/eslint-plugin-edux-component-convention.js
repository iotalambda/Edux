/**
 * @type { import("@typescript-eslint/utils").TSESLint.RuleModule<"badComponentFileName" | "pageDirHasSubDirs", []> }
 */
const rule = {
  meta: {
    type: "problem",
    docs: {
      description: "EDUX: Enforce component file naming and directory structure",
    },
    messages: {
      badComponentFileName:
        "EDUX: File `{{name}}` is in directory `{{componentsDir}}`, so its name must start with `{{prefix}}_` and end with `_Cont.ts` or `_Pres.tsx`.",
      componentsDirHasSubDirs:
        "EDUX: Directory `{{componentsDir}}` must not have subdirectories. It is only allowed to have component files.",
    },
    schema: [],
  },
  create(context) {
    const filePath = context.physicalFilename;
    if (!filePath.includes("/components/")) return {};

    const parts = filePath.split("/");
    const componentsIx = parts.indexOf("components");
    const afterComponents = parts.splice(componentsIx + 1);
    const componentsDir = parts.join("/");
    if (afterComponents.length > 1) {
      return {
        Program(node) {
          context.report({
            node,
            messageId: "componentsDirHasSubDirs",
            data: {
              componentsDir,
            },
          });
        },
      };
    }

    let prefix = parts[parts.length - 2];
    prefix = prefix.charAt(0).toUpperCase() + prefix.slice(1);
    const name = afterComponents[0];
    if (!name.startsWith(`${prefix}_`) || (!name.endsWith("_Cont.ts") && !name.endsWith("_Pres.tsx"))) {
      return {
        Program(node) {
          context.report({
            node,
            messageId: "badComponentFileName",
            data: {
              componentsDir,
              prefix,
              name,
            },
          });
        },
      };
    }
    return {};
  },
};

export default rule;
