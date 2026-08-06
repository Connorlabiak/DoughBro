# React & TypeScript Web App Guidelines

## 1. Stack & Tools
- **Framework:** React with TypeScript (Vite/SPA) located in `DoughBro/Frontend/React`.
- **Nginx:** Serves the React SPA in production.
- **Styling:** Tailwind CSS + Shadcn UI components.

## 2. Directory Structure Conventions
Keep code organized by feature and UI responsibility:
- `/src/components/ui` -> Reusable Shadcn base primitives (Button, Card, Dialog, etc.).
- `/src/components/categorization` -> Drag-and-drop batch categorization components.
- `/src/components/summary` -> Spending trend charts and customizable metrics.
- `/src/components/categories` -> Category management modals and lists.
- `/src/services` -> REST API integration layers talking to the .NET Backend.
- `/src/types` -> TypeScript interfaces mirroring backend DTOs.
- `/src/context` -> Global state (Auth state, active filters).

## 3. Data Flow & Backend Alignment
- **DTO Alignment:** All API data models **MUST** match backend camelCase DTOs (e.g., `TransactionDTO`, `CategoryDTO`).
- **Security:** Always use `/src/lib/apiClient.ts` to include JWT from Firebase Auth in `Authorization: Bearer <token>` header instead of fetch.
- **No Direct Firebase Firestore Calls:** The frontend communicates strictly with the .NET Web API backend.

## 4. Key UI/UX Specifications

Everything should exist in wigets within the same dashboard page in `/src/components/Dashboard.tsx`. Smaller functions shuold exist within modals such as add transaction or add category.

### A. Drag-and-Drop Categorization Widget (Core Selling Point)
- Present uncategorized transactions sequentially in a queue or stack.
- Transactions should have edit icon for editing details (amount, date, description, name).
- Allow users to drag transactions onto category cards for fast batch processing.
- Provide clear visual cues, smooth animations, and an instant "Undo" action.

### B. Summary & Analytics Widget
- Highly customizable spending trend graphs (Group by: Week, Month, Category).
- Filterable date ranges and interactive chart tooltips.

### C. Category Management Widget
- Allow users to create, edit, reorder, color-code categories.

## 5. Coding & Style Rules
- Use functional components with strict TypeScript types/interfaces for all props.
- **Avoid `any` types.** Define explicit interfaces in `/src/types/`.
- Prefer Tailwind utility classes; avoid inline styles.
- Keep UI components clean—delegate complex logic to custom hooks (e.g., `useCategorization`, `useTransactions`).
- Avoid style in html components; use Tailwind classes or Shadcn UI components.

## 6. General Output Rules for Codex (ALWAYS FOLLOW)
- **Track and Refer to Changes:** Load `@.codex/webStatus.md` when working on frontend tasks. Update the file with a concise summary of changes made for each task.