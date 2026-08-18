import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { getCategoryColorClasses } from "@/lib/categoryColors";
import { cn } from "@/lib/utils";
import { deleteCategory, getCategoryTransactions, updateCategory } from "@/services/categoryService";
import type { Category, Transaction } from "@/types/api";
import { useEffect, useState } from "react";
import type { FormEvent } from "react";

interface CategoryDetailsModalProps {
    category: Category;
    onClose: () => void;
    onCategoryUpdated: (category: Category) => void;
    onCategoryDeleted: (categoryId: string) => void;
}

export function CategoryDetailsModal({ category, onClose, onCategoryUpdated, onCategoryDeleted }: CategoryDetailsModalProps) {
    const [transactions, setTransactions] = useState<Transaction[]>([]);
    const [name, setName] = useState(category.name);
    const [isLoading, setIsLoading] = useState(true);
    const [isSaving, setIsSaving] = useState(false);
    const [isDeleting, setIsDeleting] = useState(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        const loadTransactions = async () => {
            setIsLoading(true);
            setError(null);

            try {
                const result = await getCategoryTransactions(category.id);
                setTransactions(result.sort((a, b) => b.date.localeCompare(a.date)));
            } catch (err) {
                console.error("Failed to load category transactions:", err);
                setError("Could not load this category's transactions.");
            } finally {
                setIsLoading(false);
            }
        };

        void loadTransactions();
    }, [category.id]);

    const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        if (!name.trim()) {
            setError("Category name is required.");
            return;
        }

        setIsSaving(true);
        setError(null);
        try {
            const updatedCategory = await updateCategory(category.id, { name: name.trim() });
            onCategoryUpdated(updatedCategory);
        } catch (err) {
            console.error("Failed to update category:", err);
            setError("Could not update this category.");
        } finally {
            setIsSaving(false);
        }
    };

    const handleDelete = async () => {
        if (!window.confirm(`Delete ${category.name}? Its transactions will become uncategorized.`)) {
            return;
        }

        setIsDeleting(true);
        setError(null);
        try {
            await deleteCategory(category.id);
            onCategoryDeleted(category.id);
        } catch (err) {
            console.error("Failed to delete category:", err);
            setError("Could not delete this category.");
        } finally {
            setIsDeleting(false);
        }
    };

    const isBusy = isSaving || isDeleting;
    const colorClasses = getCategoryColorClasses(category.colorId);

    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-zinc-950/50 p-4" role="dialog" aria-modal="true" aria-labelledby="category-details-title">
            <div className="flex max-h-[calc(100vh-2rem)] w-full max-w-2xl flex-col border border-zinc-200 bg-white shadow-2xl">
                <div className={cn("flex items-start justify-between gap-4 border-b border-zinc-200 p-6", colorClasses.card)}>
                    <div>
                        <h2 id="category-details-title" className="text-xl font-semibold tracking-tight">{category.name}</h2>
                        <p className="text-sm text-zinc-700">{transactions.length} transactions</p>
                    </div>
                    <Button type="button" variant="outline" size="sm" onClick={onClose} disabled={isBusy}>Close</Button>
                </div>

                <div className="overflow-y-auto p-6">
                    <form onSubmit={handleSubmit} className="mb-6 flex gap-3 border-b border-zinc-200 pb-6">
                        <div className="min-w-0 flex-1">
                            <label className="mb-2 block text-sm font-semibold text-zinc-700" htmlFor="edit-category-name">Category Name</label>
                            <Input id="edit-category-name" value={name} onChange={(event) => setName(event.target.value)} disabled={isBusy} required />
                        </div>
                        <Button type="submit" className="mt-7" disabled={isBusy}>{isSaving ? "Saving..." : "Save"}</Button>
                    </form>

                    <div className="mb-3 flex items-center justify-between gap-3">
                        <h3 className="text-sm font-semibold uppercase tracking-wide text-zinc-500">Transactions</h3>
                        <Button type="button" variant="destructive" size="sm" onClick={handleDelete} disabled={isBusy}>
                            {isDeleting ? "Deleting..." : "Delete Category"}
                        </Button>
                    </div>

                    {error && <p className="mb-4 border border-red-200 bg-red-50 px-4 py-3 text-sm font-medium text-red-700">{error}</p>}

                    {isLoading ? (
                        <p className="border border-dashed border-zinc-300 p-5 text-center text-sm text-zinc-500">Loading transactions...</p>
                    ) : transactions.length > 0 ? (
                        <ul className="divide-y divide-zinc-200 border border-zinc-200">
                            {transactions.map((transaction) => (
                                <li key={transaction.id} className="flex items-center justify-between gap-4 p-4">
                                    <div className="min-w-0">
                                        <p className="truncate font-semibold">{transaction.merchantName || transaction.name}</p>
                                        <p className="mt-1 text-sm text-zinc-500">{formatDate(transaction.date)}</p>
                                    </div>
                                    <p className="shrink-0 font-semibold">{formatAmount(transaction.amount)}</p>
                                </li>
                            ))}
                        </ul>
                    ) : (
                        <p className="border border-dashed border-zinc-300 p-5 text-center text-sm text-zinc-500">No transactions in this category.</p>
                    )}
                </div>
            </div>
        </div>
    );
}

function formatAmount(amount: number) {
    return new Intl.NumberFormat("en-US", { style: "currency", currency: "USD" }).format(amount);
}

function formatDate(date: string) {
    return new Intl.DateTimeFormat("en-US", { month: "short", day: "numeric", year: "numeric" }).format(new Date(`${date}T00:00:00`));
}
