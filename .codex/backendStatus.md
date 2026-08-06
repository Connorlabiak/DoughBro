# DoughBro - Project Status & Roadmap

## Currently Working On

## Recently Completed
- [x] Added Firestore-backed user category creation and retrieval APIs.
- [x] Added standardized category color palette API with per-user usage tracking.
- [x] Added atomic category creation/color reservation using user color usage documents.
- [x] Removed non-implemented transaction service methods from interfaces and classes.
- [x] Removed missing category service/repository registrations from backend DI.
- [x] Added XML comments for backend service and repository interface methods.
- [x] Implemented backend endpoint for transaction retrieval.
- [x] Deployed Backend to Google Cloud Run with CI/CD pipeline.
- [x] Implemented Plaid Sync for transactions.
- [x] Implemented Plaid Link flow for user account linking.
- [x] Implemented JWT validation for Firebase Auth in backend API.
- [x] Defined data structures for transactions and user. 

## Active Technical Stack & State
- **Current Database Collections:** `users`, `transactions`, `categories`, `category_color_usage`
- **Auth Flow:** Frontend gets JWT from Firebase -> Sends in `Authorization: Bearer <token>` header -> Backend validates UID.
- **TransactionSync Flow:** Plaid API -> Backend -> Firestore `transactions` collection.
- **Transaction Retrieval Flow:** Frontend -> Backend -> Firestore `transactions` collection.
- **Category Management Flow:** Frontend -> Backend -> Firestore user `categories` and `category_color_usage` collections.

## Known Bugs / Tech Debt
- Need to implement Plaid webhook handler for real-time transaction updates.
