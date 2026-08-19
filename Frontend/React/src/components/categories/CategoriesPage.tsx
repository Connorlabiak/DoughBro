import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { auth } from "@/firebase/firebase";
import { getCategoryColorClasses } from "@/lib/categoryColors";
import { cn } from "@/lib/utils";
import { createCategory, getCategories, getCategoryColors } from "@/services/categoryService";
import type { Category, CategoryColor } from "@/types/api";
import { signOut } from "firebase/auth";
import { useEffect, useMemo, useState } from "react";
import type { FormEvent } from "react";
import { useNavigate } from "react-router-dom";
import { CategoryDetailsModal } from "./CategoryDetailsModal";

export default function CategoriesPage() {
    const navigate = useNavigate();
    const [categories, setCategories] = useState<Category[]>([]);
    const [colors, setColors] = useState<CategoryColor[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [selectedCategory, setSelectedCategory] = useState<Category | null>(null);
    const [categoryName, setCategoryName] = useState("");
    const [selectedColorId, setSelectedColorId] = useState("");
    const [isSaving, setIsSaving] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const firstUnusedColorId = useMemo(
        () => colors.find((color) => !color.isUsed)?.id ?? "",
        [colors],
    );

    const loadCategories = async () => {
        setIsLoading(true);
        setError(null);

        try {
            const [categoryResult, colorResult] = await Promise.all([
                getCategories(),
                getCategoryColors(),
            ]);
            setCategories(categoryResult);
            setColors(colorResult);
            setSelectedColorId((currentColorId) => currentColorId || colorResult.find((color) => !color.isUsed)?.id || "");
        } catch (err) {
            console.error("Failed to load categories:", err);
            setError("Could not load categories right now.");
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        void loadCategories();
    }, []);

    const openAddModal = () => {
        setCategoryName("");
        setSelectedColorId(firstUnusedColorId);
        setError(null);
        setIsModalOpen(true);
    };

    const closeAddModal = () => {
        if (isSaving) {
            return;
        }

        setIsModalOpen(false);
        setCategoryName("");
        setSelectedColorId(firstUnusedColorId);
    };

    const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        if (!categoryName.trim() || !selectedColorId) {
            setError("Choose a name and an unused color.");
            return;
        }

        setIsSaving(true);
        setError(null);

        try {
            const category = await createCategory({
                name: categoryName.trim(),
                colorId: selectedColorId,
            });

            setCategories((currentCategories) => [...currentCategories, category].sort((a, b) => a.name.localeCompare(b.name)));
            setColors((currentColors) =>
                currentColors.map((color) =>
                    color.id === selectedColorId ? { ...color, isUsed: true } : color,
                ),
            );
            setIsModalOpen(false);
            setCategoryName("");
            setSelectedColorId(colors.find((color) => !color.isUsed && color.id !== selectedColorId)?.id ?? "");
        } catch (err) {
            console.error("Failed to create category:", err);
            setError("Could not create that category. Try another name or color.");
        } finally {
            setIsSaving(false);
        }
    };

    return (
        <div className="min-h-screen bg-zinc-50 text-zinc-950">
            <header className="flex items-center justify-between border-b border-zinc-200 bg-white px-6 py-4">
                <div>
                    <h1 className="text-2xl font-semibold tracking-tight">Categories</h1>
                    <p className="text-sm text-zinc-500">{categories.length} categories</p>
                </div>
                <div className="flex flex-wrap items-center justify-end gap-3">
                    <Button variant="outline" onClick={() => navigate("/dashboard")}>Dashboard</Button>
                    <Button onClick={openAddModal} disabled={!firstUnusedColorId}>Add Category</Button>
                    <Button variant="outline" onClick={() => logout()}>Logout</Button>
                </div>
            </header>

            <main className="p-6">
                {error && !isModalOpen && (
                    <p className="mb-4 border border-red-200 bg-red-50 px-4 py-3 text-sm font-medium text-red-700">
                        {error}
                    </p>
                )}

                {isLoading ? (
                    <div className="border border-dashed border-zinc-300 bg-white p-8 text-center text-sm text-zinc-500">
                        Loading categories...
                    </div>
                ) : categories.length > 0 ? (
                    <section className="grid grid-cols-1 gap-4 sm:grid-cols-2 lg:grid-cols-4">
                        {categories.map((category) => (
                            <article
                                key={category.id}
                                onClick={() => setSelectedCategory(category)}
                                className={cn(
                                    "flex min-h-32 cursor-pointer items-center justify-center border-2 p-5 text-center shadow-lg transition-all duration-150 ease-out will-change-transform hover:scale-[1.02] hover:shadow-2xl",
                                    getCategoryColorClasses(category.colorId).card,
                                )}
                            >
                                <h2 className="text-xl font-bold text-zinc-950">{category.name}</h2>
                            </article>
                        ))}
                    </section>
                ) : (
                    <div className="border border-dashed border-zinc-300 bg-white p-8 text-center">
                        <p className="text-sm font-medium text-zinc-700">No categories yet.</p>
                    </div>
                )}
            </main>

            {isModalOpen && (
                <div className="fixed inset-0 z-50 flex items-center justify-center bg-zinc-950/50 p-4">
                    <form onSubmit={handleSubmit} className="w-full max-w-lg border border-zinc-200 bg-white p-6 shadow-2xl">
                        <div className="mb-5 flex items-start justify-between gap-4">
                            <div>
                                <h2 className="text-xl font-semibold tracking-tight">Add Category</h2>
                                <p className="text-sm text-zinc-500">Choose a name and one available color.</p>
                            </div>
                            <Button type="button" variant="outline" size="sm" onClick={closeAddModal} disabled={isSaving}>
                                Close
                            </Button>
                        </div>

                        <label className="block text-sm font-semibold text-zinc-700" htmlFor="category-name">
                            Category Name
                        </label>
                        <Input
                            id="category-name"
                            value={categoryName}
                            onChange={(event) => setCategoryName(event.target.value)}
                            className="mb-5"
                            placeholder="Restaurants"
                            required
                        />

                        <fieldset className="mb-5">
                            <legend className="mb-3 text-sm font-semibold text-zinc-700">Color</legend>
                            <div className="grid grid-cols-2 gap-3 sm:grid-cols-4">
                                {colors.map((color) => {
                                    const isSelected = selectedColorId === color.id;
                                    const colorClasses = getCategoryColorClasses(color.id);

                                    return (
                                        <button
                                            key={color.id}
                                            type="button"
                                            disabled={color.isUsed}
                                            onClick={() => setSelectedColorId(color.id)}
                                            className={cn(
                                                "flex h-20 flex-col items-center justify-center border-2 text-sm font-semibold transition",
                                                colorClasses.card,
                                                color.isUsed ? "cursor-not-allowed opacity-35" : "hover:scale-[1.03]",
                                                isSelected && `scale-[1.04] shadow-lg ${colorClasses.selected}`,
                                            )}
                                        >
                                            <span className={cn("mb-2 block size-5", colorClasses.swatch)} />
                                            {color.name}
                                        </button>
                                    );
                                })}
                            </div>
                        </fieldset>

                        {error && (
                            <p className="mb-4 border border-red-200 bg-red-50 px-4 py-3 text-sm font-medium text-red-700">
                                {error}
                            </p>
                        )}

                        <div className="flex justify-end gap-3">
                            <Button type="button" variant="outline" onClick={closeAddModal} disabled={isSaving}>
                                Cancel
                            </Button>
                            <Button type="submit" disabled={isSaving || !firstUnusedColorId}>
                                {isSaving ? "Adding..." : "Add Category"}
                            </Button>
                        </div>
                    </form>
                </div>
            )}

            {selectedCategory && (
                <CategoryDetailsModal
                    category={selectedCategory}
                    onClose={() => setSelectedCategory(null)}
                    onCategoryUpdated={(updatedCategory) => {
                        setCategories((currentCategories) => currentCategories.map((category) =>
                            category.id === updatedCategory.id ? updatedCategory : category,
                        ));
                        setSelectedCategory(updatedCategory);
                    }}
                    onCategoryDeleted={(categoryId) => {
                        setCategories((currentCategories) => currentCategories.filter((category) => category.id !== categoryId));
                        setColors((currentColors) => currentColors.map((color) =>
                            color.id === selectedCategory.colorId ? { ...color, isUsed: false } : color,
                        ));
                        setSelectedCategory(null);
                    }}
                />
            )}
        </div>
    );
}

async function logout() {
    await signOut(auth);
}
