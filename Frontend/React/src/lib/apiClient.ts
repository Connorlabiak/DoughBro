import { getAuth } from "firebase/auth";

export async function apiFetch<T>(endpoint: string, options: RequestInit = {}): Promise<T> {
    const auth = getAuth();
    const user = auth.currentUser;

    const headers = new Headers(options.headers);
    headers.set("Content-Type", "application/json");

    if (user) {
        const token = await user.getIdToken();
        headers.set("Authorization", `Bearer ${token}`);
    }

    const response = await fetch(`${endpoint}`, {
        ...options,
        headers,
    });

    if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`API Error [${response.status}]: ${errorText}`);
    }

    if (response.status === 204) {
        return undefined as T;
    }

    return response.json();
}
