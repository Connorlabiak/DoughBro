import { Button } from "@/components/ui/button";
import { signOut } from "firebase/auth";
import { auth } from "@/firebase/firebase";
import { PlaidLinkButton } from "@/components/PlaidLinkButton";
import { SyncTransactionsButton } from "./SyncTransactionsButton";
import { CategoryDetailsModal } from "./categories/CategoryDetailsModal";
import { useEffect, useMemo, useState } from "react";
import type { DragEvent } from "react";
import { useNavigate } from "react-router-dom";
import type { Category, Transaction } from "@/types/api";
import { getCategoryColorClasses } from "@/lib/categoryColors";
import { cn } from "@/lib/utils";
import { getCategories } from "@/services/categoryService";
import { getTransactions, updateTransactionCategory } from "@/services/transactionService";

const UNCATEGORIZED_VALUES = new Set([undefined, null, "", "unsorted"]);

export default function Dashboard() {
    const navigate = useNavigate();
    const [categories, setCategories] = useState<Category[]>([]);
    const [transactions, setTransactions] = useState<Transaction[]>([]);
    const [activeTransactionId, setActiveTransactionId] = useState<string | null>(null);
    const [hoveredCategoryId, setHoveredCategoryId] = useState<string | null>(null);
    const [selectedCategory, setSelectedCategory] = useState<Category | null>(null);
    const [categoryPage, setCategoryPage] = useState(0);
    const [isLoading, setIsLoading] = useState(true);
    const [isUpdating, setIsUpdating] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const uncategorizedTransactions = useMemo(
        () => transactions.filter((transaction) => UNCATEGORIZED_VALUES.has(transaction.category)),
        [transactions],
    );

    const activeTransaction = uncategorizedTransactions[0];
    const categorizedCount = transactions.length - uncategorizedTransactions.length;
    const categoryPageCount = Math.ceil(categories.length / 8);
    const visibleCategories = categories.slice(categoryPage * 8, (categoryPage + 1) * 8);

    const loadDashboardData = async () => {
        setIsLoading(true);
        setError(null);

        try {
            const [categoryResult, transactionResult] = await Promise.all([
                getCategories(),
                getTransactions(100),
            ]);
            setCategories(categoryResult);
            setTransactions(transactionResult);
        } catch (err) {
            console.error("Failed to load dashboard data:", err);
            setError("Could not load transactions right now.");
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        void loadDashboardData();
    }, []);

    useEffect(() => {
        setCategoryPage((currentPage) => Math.min(currentPage, Math.max(categoryPageCount - 1, 0)));
    }, [categoryPageCount]);

    const handleDragStart = (event: DragEvent<HTMLDivElement>, transactionId?: string) => {
        if (!transactionId || isUpdating) {
            event.preventDefault();
            return;
        }

        setActiveTransactionId(transactionId);
        event.dataTransfer.effectAllowed = "move";
        event.dataTransfer.setData("text/plain", transactionId);
    };

    const handleDragOver = (event: DragEvent<HTMLDivElement>, categoryId: string) => {
        event.preventDefault();
        setHoveredCategoryId(categoryId);
        event.dataTransfer.dropEffect = "move";
    };

    const handleDragLeave = (event: DragEvent<HTMLDivElement>, categoryId: string) => {
        const nextTarget = event.relatedTarget;
        if (nextTarget instanceof Node && event.currentTarget.contains(nextTarget)) {
            return;
        }

        setHoveredCategoryId((currentCategoryId) =>
            currentCategoryId === categoryId ? null : currentCategoryId,
        );
    };

    const handleDragEnd = () => {
        setActiveTransactionId(null);
        setHoveredCategoryId(null);
    };

    const handleDrop = async (event: DragEvent<HTMLDivElement>, category: Category) => {
        event.preventDefault();

        const transactionId = event.dataTransfer.getData("text/plain") || activeTransactionId;
        if (!transactionId) {
            return;
        }

        setIsUpdating(true);
        setError(null);

        try {
            await updateTransactionCategory(transactionId, category.id);
            setTransactions((currentTransactions) =>
                currentTransactions.map((transaction) =>
                    transaction.id === transactionId
                        ? { ...transaction, category: category.id }
                        : transaction,
                ),
            );
        } catch (err) {
            console.error("Failed to update transaction category:", err);
            setError("Could not update that transaction category.");
        } finally {
            setActiveTransactionId(null);
            setHoveredCategoryId(null);
            setIsUpdating(false);
        }
    };

    return (
        <div className="min-h-screen bg-zinc-50 text-zinc-950">
            <header className="flex items-center justify-between border-b border-zinc-200 bg-white px-6 py-4">
                <div>
                    <h1 className="text-2xl font-semibold tracking-tight">Dashboard</h1>
                    <p className="text-sm text-zinc-500">
                        {categorizedCount} categorized / {uncategorizedTransactions.length} waiting
                    </p>
                </div>
                <div className="flex flex-wrap items-center justify-end gap-3">
                    <Button variant="outline" onClick={() => navigate("/categories")}>Categories</Button>
                    <PlaidLinkButton />
                    <SyncTransactionsButton onSyncCompleteCallback={loadDashboardData} />
                    <Button variant="outline" onClick={() => logout()}>Logout</Button>
                </div>
            </header>

            <main className="grid min-h-[calc(100vh-81px)] grid-cols-1 gap-6 p-6 xl:grid-cols-[minmax(320px,420px)_1fr]">
                <section className="flex min-h-[360px] flex-col justify-center border border-zinc-200 bg-white p-5 shadow-sm">
                    <div className="mb-5 flex items-center justify-between">
                        <h2 className="text-sm font-semibold uppercase tracking-wide text-zinc-500">Next Transaction</h2>
                        {isUpdating && <span className="text-xs font-medium uppercase tracking-wide text-zinc-500">Saving</span>}
                    </div>

                    {isLoading ? (
                        <div className="border border-dashed border-zinc-300 p-6 text-center text-sm text-zinc-500">
                            Loading transactions...
                        </div>
                    ) : activeTransaction ? (
                        <div
                            draggable={!isUpdating}
                            onDragStart={(event) => handleDragStart(event, activeTransaction.id)}
                            onDragEnd={handleDragEnd}
                            className="cursor-grab border border-zinc-300 bg-zinc-950 p-5 text-white shadow-lg transition duration-150 ease-out active:scale-[0.99] active:cursor-grabbing active:opacity-95"
                        >
                            <div className="mb-8 flex items-start justify-between gap-4">
                                <div className="min-w-0">
                                    <p className="truncate text-xl font-semibold">
                                        {activeTransaction.merchantName || "Unknown Merchant"}
                                    </p>
                                    <p className="mt-1 break-words text-sm text-zinc-300">{activeTransaction.name}</p>
                                </div>
                                <p className="shrink-0 text-xl font-semibold">
                                    {formatAmount(activeTransaction.amount)}
                                </p>
                            </div>
                            <p className="text-sm font-medium text-zinc-300">{formatDate(activeTransaction.date)}</p>
                        </div>
                    ) : (
                        <div className="border border-dashed border-emerald-300 bg-emerald-50 p-6 text-center text-sm font-medium text-emerald-700">
                            All visible transactions are categorized.
                        </div>
                    )}

                    {error && <p className="mt-4 text-sm font-medium text-red-600">{error}</p>}
                </section>

                <section className="grid grid-cols-2 gap-4 md:grid-cols-4 xl:grid-rows-2">
                    {visibleCategories.map((category) => {
                        const isHovered = hoveredCategoryId === category.id;
                        const colorClasses = getCategoryColorClasses(category.colorId);

                        return (
                            <div
                                key={category.id}
                                onDragEnter={() => setHoveredCategoryId(category.id)}
                                onDragOver={(event) => handleDragOver(event, category.id)}
                                onDragLeave={(event) => handleDragLeave(event, category.id)}
                                onDrop={(event) => handleDrop(event, category)}
                                onClick={() => setSelectedCategory(category)}
                                className={cn(
                                    "flex min-h-36 cursor-pointer items-center justify-center border-2 p-4 text-center shadow-sm",
                                    "transition-all duration-150 ease-out will-change-transform",
                                    colorClasses.card,
                                    isHovered ? "z-10 scale-[1.06] shadow-2xl" : "hover:scale-[1.02] hover:shadow-lg",
                                )}
                            >
                                <span className="text-lg font-bold text-zinc-950">{category.name}</span>
                            </div>
                        );
                    })}
                </section>
            </main>

            {categoryPageCount > 1 && (
                <Button
                    type="button"
                    size="icon"
                    aria-label={categoryPage === 0 ? "Show more categories" : "Show previous categories"}
                    onClick={() => setCategoryPage((currentPage) => currentPage === 0 ? 1 : 0)}
                    className="fixed bottom-6 right-6 z-40 rounded-full shadow-xl transition-all duration-200 hover:scale-110"
                >
                    <span className="text-lg leading-none" aria-hidden="true">{categoryPage === 0 ? "→" : "←"}</span>
                </Button>
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
                        setTransactions((currentTransactions) => currentTransactions.map((transaction) =>
                            transaction.category === categoryId ? { ...transaction, category: undefined } : transaction,
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

function formatAmount(amount: number) {
    return new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "USD",
    }).format(amount);
}

function formatDate(date: string) {
    return new Intl.DateTimeFormat("en-US", {
        month: "short",
        day: "numeric",
        year: "numeric",
    }).format(new Date(`${date}T00:00:00`));
}
