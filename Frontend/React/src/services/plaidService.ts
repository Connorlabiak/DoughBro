import { apiFetch } from "@/lib/apiClient";
import type {
    ExchangePublicTokenRequest,
    ExchangePublicTokenResponse,
    LinkTokenResponse,
} from "@/types/api";

export function createLinkToken() {
    return apiFetch<LinkTokenResponse>("/api/plaid/create-link-token", {
        method: "POST",
    });
}

export function exchangePublicToken(request: ExchangePublicTokenRequest) {
    return apiFetch<ExchangePublicTokenResponse>("/api/plaid/exchange-public-token", {
        method: "POST",
        body: JSON.stringify(request),
    });
}
