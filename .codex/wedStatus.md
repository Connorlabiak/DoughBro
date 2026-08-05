# DoughBro React Web App - Status & Roadmap

## Currently Working On

## Recently Completed
- [x] Initialized React + TypeScript + Vite project in `DoughBro/Frontend/React`.
- [x] Configured Tailwind CSS and Shadcn UI component library.
- [x] Implemented Firebase Authentication context and login UI (`/src/context/AuthContext.tsx`).
- [x] Established REST API service wrapper (`/src/lib/apiClient.ts`) with Bearer token interception.
- [x] Deployed app to to Google Cloud run with CI/CD pipeline.
- [x] Created unstyled dashboard page for testing.

## Active Technical Stack & State
- **Login Flow:** Firebase Auth handles user login and JWT issuance in `/src/components/Login.tsx`
- **Auth Flow:** React gets JWT from Firebase Auth -> Sends in API headers to .NET Backend.
- **Plaid Link Flow:** React handles Plaid Link flow in `/src/components/PlaidLinkButton.tsx` -> Sends public token to backend for exchange. 

## Known Bugs / Tech Debt
