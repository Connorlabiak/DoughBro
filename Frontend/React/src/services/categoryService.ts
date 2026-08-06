import { apiFetch } from "@/lib/apiClient";
import type { Category, CategoryColor, CreateCategoryRequest } from "@/types/api";

export function getCategories() {
    return apiFetch<Category[]>("/api/categories");
}

export function getCategoryColors() {
    return apiFetch<CategoryColor[]>("/api/categories/colors");
}

export function createCategory(request: CreateCategoryRequest) {
    return apiFetch<Category>("/api/categories", {
        method: "POST",
        body: JSON.stringify(request),
    });
}
