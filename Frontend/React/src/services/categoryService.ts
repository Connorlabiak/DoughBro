import { apiFetch } from "@/lib/apiClient";
import type { Category, CategoryColor, CreateCategoryRequest, UpdateCategoryRequest } from "@/types/api";

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

export function updateCategory(categoryId: string, request: UpdateCategoryRequest) {
    return apiFetch<Category>(`/api/categories/${categoryId}`, {
        method: "PATCH",
        body: JSON.stringify(request),
    });
}

export function deleteCategory(categoryId: string) {
    return apiFetch<void>(`/api/categories/${categoryId}`, {
        method: "DELETE",
    });
}
