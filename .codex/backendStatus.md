# DoughBro - Project Status & Roadmap

## Currently Working On

## Recently Completed
- [x] Implemented backend endpoint for transaction retrieval.
- [x] Deployed Backend to Google Cloud Run with CI/CD pipeline.
- [x] Implemented Plaid Sync for transactions.
- [x] Implemented Plaid Link flow for user account linking.
- [x] Implemented JWT validation for Firebase Auth in backend API.
- [x] Defined data structures for transactions and user. 

## Active Technical Stack & State
- **Current Database Collections:** `users`, `transactions`
- **Auth Flow:** Frontend gets JWT from Firebase -> Sends in `Authorization: Bearer <token>` header -> Backend validates UID.
- **TransactionSync Flow:** Plaid API -> Backend -> Firestore `transactions` collection.
- **Transaction Retrieval Flow:** Frontend -> Backend -> Firestore `transactions` collection.

## Known Bugs / Tech Debt
- Need to implement Plaid webhook handler for real-time transaction updates.