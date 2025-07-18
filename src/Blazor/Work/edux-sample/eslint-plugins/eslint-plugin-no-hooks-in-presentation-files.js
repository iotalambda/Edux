/**
 * @type { import("@typescript-eslint/utils").TSESLint.RuleModule<"eduxNoHooksInPresentationFiles", []>}
 */
const rule = {
  meta: {
    type: "problem",
    docs: {
      description: "EDUX: Disallow Reach Hooks in `*_Pres.tsx`, `page.tsx` and `layout.tsx` files",
    },
    messages: {
      eduxNoHooksInPresentationFiles:
        "EDUX: Hooks are not allowed in Presentation files. Use Hooks only in Container files.",
    },
    schema: [],
  },
  create(context) {
    const filename = context.filename;
    const isPresentationFile = filename === "page.tsx" || filename === "layout.tsx" || filename.endsWith("_Pres.tsx");
    if (!isPresentationFile) return {};

    return {
      CallExpression(node) {
        const { callee } = node;
        if (callee.type === "Identifier" && /^use[A-Z0-9].*/.test(callee.name)) {
          context.report({
            node,
            messageId: "eduxNoHooksInPresentationFiles",
          });
        }
      },
    };
  },
};

export default rule;
