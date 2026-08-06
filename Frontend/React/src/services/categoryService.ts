import { apiFetch } from "@/lib/apiClient";
import type { Category } from "@/types/api";

export function getCategories() {
    return apiFetch<Category[]>("/api/categories");
}
