export interface LinkTokenResponse {
    linkToken: string;
}

export interface ExchangePublicTokenRequest {
    publicToken: string;
    institutionName: string;
}

export interface ExchangePublicTokenResponse {
    success: boolean;
    itemId: string;
}

export interface SyncTransactionsResponse {
    message: string;
}

export interface Category {
    id: string;
    name: string;
    color: string;
}

export interface Transaction {
    id?: string;
    origin: string;
    userId: string;
    name: string;
    date: string;
    amount: number;
    merchantName?: string | null;
    description?: string | null;
    category?: string | null;
    isPending: boolean;
}

export interface UpdateTransactionCategoryRequest {
    category: string;
}
