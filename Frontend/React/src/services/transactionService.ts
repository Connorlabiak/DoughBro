import { apiFetch } from "@/lib/apiClient";
import type { SyncTransactionsResponse, Transaction, UpdateTransactionCategoryRequest } from "@/types/api";

export function syncTransactions() {
    return apiFetch<SyncTransactionsResponse>("/api/transactions/sync", {
        method: "POST",
    });
}

export function getTransactions(limit = 50) {
    return apiFetch<Transaction[]>(`/api/transactions/get?limit=${limit}`);
}

export function getTransactionsByCategory(categoryId: string) {
    return apiFetch<Transaction[]>(`/api/transactions/category/${categoryId}`);
}

export function updateTransactionCategory(transactionId: string, category: string) {
    const body: UpdateTransactionCategoryRequest = { category };

    return apiFetch<void>(`/api/transactions/${transactionId}/category`, {
        method: "PATCH",
        body: JSON.stringify(body),
    });
}
