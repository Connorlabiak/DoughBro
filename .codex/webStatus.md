# DoughBro React Web App - Status & Roadmap

## Currently Working On

## Recently Completed
- [x] Seed default categories at account creation and improved dashboard category paging controls.
- [x] Added default category initialization and dashboard category paging.
- [x] Added color selection to the shared category details modal.
- [x] Added shared category details modal with transaction history, renaming, and deletion.
- [x] Added category management page with category list and add-category modal.
- [x] Added standardized category color classes for the 16-color backend palette.
- [x] Added dashboard navigation to category management.
- [x] Moved Plaid and transaction API calls into typed service modules under `/src/services`.
- [x] Added shared API response/request interfaces under `/src/types`.
- [x] Split auth context and hook modules for cleaner component exports.
- [x] Initialized React + TypeScript + Vite project in `DoughBro/Frontend/React`.
- [x] Configured Tailwind CSS and Shadcn UI component library.
- [x] Implemented Firebase Authentication context and login UI (`/src/context/AuthContext.tsx`).
- [x] Established REST API service wrapper (`/src/lib/apiClient.ts`) with Bearer token interception.
- [x] Deployed app to to Google Cloud run with CI/CD pipeline.
- [x] Created unstyled dashboard page for testing.

## Active Technical Stack & State
- **Login Flow:** Firebase Auth handles user login and JWT issuance in `/src/components/Login.tsx`
- **Auth Flow:** React gets JWT from Firebase -> Sends in API headers to .NET Backend.
- **Plaid Link Flow:** React handles Plaid Link flow in `/src/components/PlaidLinkButton.tsx` -> Sends public token to backend for exchange.
- **Category Management Flow:** React loads categories and palette usage through `/src/services/categoryService.ts`.

## Known Bugs / Tech Debt
