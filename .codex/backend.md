# .NET Core & Firestore Backend Guidelines

## 1. Strict 3-Layer Architecture
All backend features must adhere to a strict 3-layer pattern:

1. **Controller Layer:** 
   - Handles HTTP requests, responses, status codes, and model validation.
   - **NO business logic** or direct Firestore calls.
   - Only accepts and returns DTO objects (`*DTO`).
   
2. **Service Layer:**
   - Holds ALL business logic.
   - Transforms `*DTO` objects to `*Model` objects and vice versa.
   - **NO direct Firestore logic**—interacts with the database exclusively through Repository interfaces.

3. **Repository Layer:**
   - Interfaces directly with Firebase Firestore.
   - Uses `*Model` objects only.
   - Handles queries, documents, snapshots, and database operations.

## 2. Interfaces & Dependency Injection
- ALL Service and Repository classes **MUST** have a corresponding interface prefixed with `I` (e.g., `ITransactionService`, `ITransactionRepository`).
- Inject all dependencies using constructor Dependency Injection (`IServiceCollection`).
- Declare readonly dependencies using "_" prefix (e.g., `_transactionService`) and `readonly` modifier, assigning value in constructor through DI.
- Interfaces shuold use xml comments for all exposed methods.

## 3. Data Objects & Attributes

### Controller Layer Data Transfer Objects (`*DTO`)
- Named with suffix `DTO` (e.g., `TransactionDTO`, `CategoryDTO`).
- Use `[JsonPropertyName("camelCase")]` attributes on all properties.
- Use `required` modifier where applicable.

### Service and Repository Layer Objects (`*Model`)
- Named with suffix `Model` (e.g., `TransactionModel`, `CategoryModel`).
- Use `[FirestoreProperty]` attributes on all properties.
- Use `required` modifier where applicable.

## 4. Deployment & Environment
- Solution should work on local environment and be deployable to Google Cloud Run
- Secrets stored by human devs in secrets.json locally and in Google Secret Manager for deployment.
- Solution deployed from GitHub Actions CI/CD pipeline to Google Cloud Run in Docker container.

## 5. General Output Rules for Codex (ALWAYS FOLLOW)
- **Track and Refer to Changes:** load `@.codex/backendStatus.md` when working on backend tasks. Update the file with a summary of changes made for each task. 