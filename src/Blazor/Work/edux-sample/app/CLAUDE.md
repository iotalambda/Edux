# Edux conventions

- Organize code to _Presentation Components_ and _Container Components_.
- For components under `XYZ/components` directory, use naming convention `XYZ_Subject_Cont.ts` or `XYZ_Subject_Pres.tsx` (replace `XYZ` with the `page` name and `Subject` with the subject the component focuses on).
- Always add `"use client";` on top of all component files.

## Presentation Components

- Defined in files `page.tsx`, `layout.tsx` and `components/*_Pres.tsx`.
- Cannot use hooks.
- Should render Container Components and provide any props required by them, including render props.
- You should avoid creating `components/*_Pres.tsx` files simply for code modularization. Instead, prefer using `page.tsx`. Create additional `components/*_Pres.tsx` files only if otherwise a non-trivial piece of code would be duplicated at least 5 times.

## Container Components

- Defined in files `components/*_Cont.ts`.
- Cannot use JSX/TSX.
- Provides props, including render props, so that Presentation Copmponents can render it properly.

## Miscellaneous

- Always use the underscores `_`, as described above, in component names and imports.
- ONLY CREATE `components/*_Pres.tsx` FILES AS A LAST RESORT IN CASE CODE DUPLICATION HAS INCREASED ABOVE THE THRESHOLD OF 5. OTHERWISE JUST ADD THE PRESENTATION TO `page.tsx` (or `layout.tsx`).

## Example 1

### Task

The user has asked you to add a button and a counter display on the shopping cart page.

### Thinking

- The counter needs a state, so a `/shopping-cart/components/ShoppingCart_Counter_Cont.ts` Container component should be created.
- The Container component should offer a render prop, so that the Presentation component can use it.
- There is little duplication, so the presentation should be added to `/shopping-cart/page.tsx`.

### Solution

#### `/shopping-cart/components/ShoppingCart_Counter_Cont.ts`

```ts
import { useState } from "react";

export function ShoppingCart_Counter_Cont(props: {
  renderCounter: (props: {
    count: number;
    increment: () => void;
  }) => React.ReactNode;
}) {
  const [count, setCount] = useState(0);

  const increment = () => {
    setCount((prev) => prev + 1);
  };

  return props.renderCounter({ count, increment });
}
```

#### `/shopping-cart/page.tsx`

```tsx
import { ShoppingCart_Counter_Cont } from "./components/ShoppingCart_Counter_Cont";

export default function Users() {
  return (
    <div className="p-6">
      <h1 className="text-black dark:text-white text-3xl font-bold mb-6">
        Users
      </h1>

      <ShoppingCart_Counter_Cont
        renderCounter={({ count, increment }) => (
          <div className="space-y-4">
            <div className="flex items-center gap-4">
              <span className="text-black dark:text-white text-lg">
                Counter:{" "}
                <span className="font-bold text-blue-600 dark:text-blue-400">
                  {count}
                </span>
              </span>
            </div>

            <button
              onClick={increment}
              className="px-4 py-2 bg-blue-600 hover:bg-blue-700 text-white font-medium rounded-md transition-colors duration-200"
            >
              Increment Counter
            </button>
          </div>
        )}
      />
    </div>
  );
}
```
