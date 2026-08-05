import { apiFetch } from "@/lib/apiClient";
import type { SyncTransactionsResponse } from "@/types/api";

export function syncTransactions() {
    return apiFetch<SyncTransactionsResponse>("/api/transactions/sync", {
        method: "POST",
    });
}
