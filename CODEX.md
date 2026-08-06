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
- 
## 4. General Output Rules for Codex (ALWAYS FOLLOW)
- **Be Concise:** Provide code diffs or updated code snippets directly. Avoid lengthy text explanations unless asked.
- **Preserve Conventions:** Strictly adhere to existing naming and structural patterns.
- **Do Not Re-write Unchanged Code:** Insert changes instead of rewriting large sections.
- **Avoid comments if not necessary:** Only include comments if the code is not self-explanatory or if it clarifies a complex logic.
- **Be Secure:** API endpoints shuold be authorized unless explicitly allowed. The software should be secure for compliance with Plaid API.
- **Build Verification Only:** The ONLY terminal/execution task Codex is permitted to run is verifying that the project compiles.
   - For Backend: `dotnet build`
   - For Frontend: `npm.cmd run build`
- **NO Background Processes:** NEVER start, launch, or manage dev servers (`npm run dev`, `dotnet run`, `dotnet watch`, background jobs, or HTTP listeners).
- **NO Execution Beyond Build:** Once `dotnet build` and `npm.cmd run build` pass successfully, immediately stop and hand control back to the user. Do not attempt to run or preview the app.

## 5. Mandatory Pre-Execution Git Workflow (ALWAYS ENFORCE)
Before modifying, creating, or deleting ANY files for a task:

1. **Check Current Branch:** Run `git status` or inspect the working tree.
2. **Never Touch Main/Master:** Never apply code changes directly on `master`.
3. **Automatic Task Branching:** 
   - If currently on `master`, automatically create and switch to a new branch BEFORE writing code:
     `git checkout -b feature/<short-descriptive-task-name>` (or `fix/<bug-name>`).
   - Example: For adding category API endpoints, create `feature/category-endpoints`.
4. **Automated Staging & Commit:**
   - Once the task is complete, stage the changed files and commit with a Conventional Commit message:
     `git add .`
     `git commit -m "feat(module): short description of work completed"`
5. **Update Status File:** Update the corresponding status file (`@.codex/backendStatus.md` or `@.codex/webStatus.md`) as part of the task branch's final commit.

## 6. Automatic Context Dispatch Protocol (ALWAYS FOLLOW)
Do NOT wait for the user to explicitly tag context files. Automatically read and apply the following context rules based on the user's task or file paths:

- **If the task touches C#, .NET, Web API, Controller, Service, Repository, or Firestore files (`/DoughBro/Backend`):**
  -> IMMEDIATELY load and follow `@.codex/backend.md`
  -> IMMEDIATELY load and update `@.codex/backendStatus.md`

- **If the task touches React, TypeScript, Tailwind, Shadcn, Components, or UI files (`/DoughBro/Frontend/React`):**
  -> IMMEDIATELY load and follow `@.codex/web.md`
  -> IMMEDIATELY load and update `@.codex/webStatus.md`

- **If the task touches iOS, SwiftUI, or mobile app files (`/DoughBro/Frontend/ios`):**
  -> IMMEDIATELY load and follow `@.codex/ios.md`
  -> IMMEDIATELY load and update `@.codex/iosStatus.md`

- **If the task spans both Backend and Frontend:**
  -> Load BOTH `@.codex/backend.md` and `@.codex/web.md`