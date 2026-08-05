# Finance Tracker - Codex Instructions

## 1. Project Core & Selling Point
A personal finance tracking app that syncs transactions via Plaid API and allows manual transaction entry.
- **Key Feature:** A sequential drag-and-drop categorization workflow (groceries, eating out, rent, etc.).
- **Summary Page:** Customizable spending trends and graphs (by category, week, month, etc.).
- **Category Management:** Custom category creation and management.

## 2. Core Stack
- **Backend:** .NET Core Web API, Firebase Firestore, Firebase Auth.
### Frontend:
- **Web App:** React, TypeScript, Tailwind CSS / Shadcn UI.
- **iOS APP** SwiftUI --> Future work only.

## 3. Modular Architecture Context (Reference as needed)
- For C# / .NET / Firestore / API / Backend tasks, load: `@.codex/backend.md`. Work in DoughBro/Backend
- For Web app / React tasks, load: `@.codex/web.md`. Work in DoughBro/Frontend/React

Each module has its own Dockerfile and can be deployed independently. The backend module is the only one that interacts with Firestore and Plaid API. The frontend module is a React SPA that communicates with the backend via REST API.

## 4. General Output Rules for Codex (ALWAYS FOLLOW)
- **Be Concise:** Provide code diffs or updated code snippets directly. Avoid lengthy text explanations unless asked.
- **Preserve Conventions:** Strictly adhere to existing naming and structural patterns.
- **Do Not Re-write Unchanged Code:** Insert changes instead of rewriting large sections.
- **Avoid comments if not necessary:** Only include comments if the code is not self-explanatory or if it clarifies a complex logic.
- **Be Secure:** API endpoints shuold be authorized unless explicitly allowed. The software should be secure for compliance with Plaid API.