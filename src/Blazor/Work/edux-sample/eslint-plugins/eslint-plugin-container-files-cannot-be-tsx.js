/**
 * @type { import("@typescript-eslint/utils").TSESLint.RuleModule<"eduxContainerFilesCannotBeTsx", []>}
 */
const rule = {
  meta: {
    type: "problem",
    docs: {
      description: "EDUX: Disallow `*_Cont.tsx` files",
    },
    messages: {
      eduxContainerFilesCannotBeTsx: "EDUX: Container files cannot be `.tsx`. Use `.ts` instead.",
    },
    schema: [],
  },
  create(context) {
    const filename = context.filename;
    const isBadContainerFile = filename.endsWith("_Cont.tsx");
    if (!isBadContainerFile) return {};

    return {
      Program(node) {
        context.report({
          node,
          messageId: "eduxContainerFilesCannotBeTsx",
        });
      },
    };
  },
};

export default rule;
