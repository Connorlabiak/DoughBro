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
