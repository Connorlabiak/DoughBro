import { Button } from "@/components/ui/button";
import { DatePicker } from "@/components/ui/date-picker"
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
import { getTransactions, updateTransaction } from "@/services/transactionService";
import { Input } from "@/components/ui/input";
import { RiEyeOffLine } from "@remixicon/react";

const UNCATEGORIZED_VALUES = new Set([undefined, null, "", "unsorted"]);
const HIDDEN_CATEGORY_NAME = "Hidden";

interface TransactionDraft {
    name: string;
    merchantName: string;
    description: string;
    amount: string;
    date: string;
}

export default function Dashboard() {
    const navigate = useNavigate();
    const [categories, setCategories] = useState<Category[]>([]);
    const [transactions, setTransactions] = useState<Transaction[]>([]);
    const [transactionDraft, setTransactionDraft] = useState<TransactionDraft | null>(null);
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
    const hiddenCategory = categories.find((category) => category.name === HIDDEN_CATEGORY_NAME);
    const regularCategories = categories.filter((category) => category.name !== HIDDEN_CATEGORY_NAME);
    const categoryPageCount = Math.ceil(regularCategories.length / 8);
    const visibleCategories = regularCategories.slice(categoryPage * 8, (categoryPage + 1) * 8);

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

    useEffect(() => {
        setTransactionDraft(activeTransaction ? toTransactionDraft(activeTransaction) : null);
    }, [activeTransaction?.id]);

    const handleDragStart = (event: DragEvent<HTMLDivElement>, transactionId?: string) => {
        if (!transactionId || isUpdating || event.target !== event.currentTarget) {
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
            if (!transactionDraft || !activeTransaction || transactionId !== activeTransaction.id) {
                return;
            }

            const amount = Number(transactionDraft.amount);
            if (!transactionDraft.name.trim() || !transactionDraft.date || !Number.isFinite(amount)) {
                setError("Enter a name, valid amount, and date before categorizing this transaction.");
                return;
            }

            await updateTransaction(transactionId, {
                name: transactionDraft.name.trim(),
                merchantName: transactionDraft.merchantName.trim() || null,
                description: transactionDraft.description.trim() || null,
                amount,
                date: transactionDraft.date,
                category: category.id,
            });
            setTransactions((currentTransactions) =>
                currentTransactions.map((transaction) =>
                    transaction.id === transactionId
                        ? {
                            ...transaction,
                            name: transactionDraft.name.trim(),
                            merchantName: transactionDraft.merchantName.trim() || null,
                            description: transactionDraft.description.trim() || null,
                            amount,
                            date: transactionDraft.date,
                            category: category.id,
                        }
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
        <div className="min-h-screen bg-white text-zinc-950">
            <header className="flex items-center justify-between border-b border-zinc-200 bg-white px-6 py-4">
                <div>
                    <h1 className="text-2xl font-semibold tracking-tight">Dashboard</h1>
                    <p className="text-sm text-zinc-500">
                        {categorizedCount} categorized / {uncategorizedTransactions.length} waiting
                    </p>
                </div>
                <div className="flex flex-wrap items-center justify-end gap-3">
                    <Button variant="outline" className="border-[#BF00FF] text-[#BF00FF] hover:bg-[#BF00FF]/10 hover:text-[#BF00FF]" onClick={() => navigate("/categories")}>Categories</Button>
                    <PlaidLinkButton />
                    <SyncTransactionsButton onSyncCompleteCallback={loadDashboardData} />
                    <Button variant="outline" className="border-[#BF00FF] text-[#BF00FF] hover:bg-[#BF00FF]/10 hover:text-[#BF00FF]" onClick={() => logout()}>Logout</Button>
                </div>
            </header>

            <main className="grid min-h-[calc(100vh-81px)] grid-cols-1 gap-6 p-6 xl:grid-cols-[minmax(320px,420px)_1fr]">
                <section className="relative min-h-[360px] border border-zinc-200 bg-white p-5">
                    <div className="mb-5 flex items-center justify-between">
                        <h2 className="text-sm font-semibold uppercase tracking-wide text-zinc-500">Next Transaction</h2>
                        {isUpdating && <span className="text-xs font-medium uppercase tracking-wide text-[#BF00FF]">Saving</span>}
                    </div>
                    {isLoading ? (
                        <div className="border border-dashed border-zinc-300 p-6 text-center text-sm text-zinc-500">
                            Loading transactions...
                        </div>
                    ) : activeTransaction && transactionDraft ? (
                        <div
                            draggable={!isUpdating}
                            onDragStart={(event) => handleDragStart(event, activeTransaction.id)}
                            onDragEnd={handleDragEnd}
                            className="w-full max-w-md cursor-grab border-2 border-[#BF00FF]/30 bg-white p-4 shadow-md transition duration-150 ease-out hover:bg-[#BF00FF]/10 hover:border-[#BF00FF] active:scale-[0.99] active:cursor-grabbing active:opacity-95"
                        >
                            <div className="flex items-start justify-between gap-3">
                                <div className="min-w-0 flex-1 space-y-0.5">
                                    <Input
                                        aria-label="Merchant"
                                        title={transactionDraft.merchantName}
                                        value={transactionDraft.merchantName}
                                        placeholder="Merchant"
                                        onChange={(event) => setTransactionDraft((current) => current && { ...current, merchantName: event.target.value })}
                                        className="h-auto w-full min-w-0 truncate border-transparent bg-transparent p-0 text-lg font-bold text-zinc-900 placeholder:text-zinc-400 focus-visible:border-[#BF00FF] focus-visible:ring-0 md:text-lg"
                                    />
                                    <Input
                                        aria-label="Transaction name"
                                        value={transactionDraft.name}
                                        onChange={(event) => setTransactionDraft((current) => current && { ...current, name: event.target.value })}
                                        className="h-auto p-0 border-transparent bg-transparent text-xs font-medium text-zinc-800 focus-visible:border-[#BF00FF] focus-visible:ring-0"
                                    />
                                    <Input
                                        aria-label="Description"
                                        value={transactionDraft.description}
                                        placeholder="Click to add description"
                                        onChange={(event) => setTransactionDraft((current) => current && { ...current, description: event.target.value })}
                                        className="h-auto p-0 border-transparent bg-transparent text-xs text-zinc-700 placeholder:text-zinc-600 focus-visible:border-[#BF00FF] focus-visible:ring-0"
                                    />
                                </div>
                                    <div className="flex shrink-0 items-center rounded-md bg-[#BF00FF]/10 px-2 py-1 text-base font-bold text-[#BF00FF] md:text-lg">
                                        <span aria-hidden="true" className="mr-0.5">$</span>
                                        <Input
                                            aria-label="Amount"
                                            type="text"
                                            inputMode="decimal"
                                            maxLength={12}
                                            value={transactionDraft.amount}
                                            onChange={(event) => {
                                                const val = event.target.value;
                                                if (/^-?\d{0,9}(\.\d{0,2})?$/.test(val) || val === "") {
                                                    setTransactionDraft((current) => current && { ...current, amount: val });
                                                }
                                            }}
                                            style={{ width: `${Math.max(transactionDraft.amount.toString().length, 3)}ch` }}
                                            className="h-auto p-0 border-transparent bg-transparent text-left text-base font-bold text-[#BF00FF] focus-visible:border-[#BF00FF] focus-visible:ring-0 md:text-lg"
                                        />
                                    </div>
                            </div>

                            <div className="mt-3 border-t border-zinc-100 pt-2">
                                <DatePicker
                                    value={transactionDraft.date}
                                    onChange={(newDate) =>
                                        setTransactionDraft((current) => current && { ...current, date: newDate })
                                    }
                                />
                            </div>
                        </div>
                    ) : (
                        <div className="border border-dashed border-zinc-300 bg-zinc-50 p-6 text-center text-sm font-medium text-zinc-700">
                            All visible transactions are categorized.
                        </div>
                    )}
                    {error && <p className="mt-4 text-sm font-medium text-red-600">{error}</p>}
                    {hiddenCategory && (
                        <div
                            onDragEnter={() => setHoveredCategoryId(hiddenCategory.id)}
                            onDragOver={(event) => handleDragOver(event, hiddenCategory.id)}
                            onDragLeave={(event) => handleDragLeave(event, hiddenCategory.id)}
                            onDrop={(event) => handleDrop(event, hiddenCategory)}
                            className={cn(
                                "absolute left-1/2 bottom-[16.6%] -translate-x-1/2 translate-y-1/2 flex flex-col items-center justify-center text-zinc-400 transition-all duration-150 ease-out",
                                hoveredCategoryId === hiddenCategory.id ? "scale-110 text-[#BF00FF]" : "hover:scale-105 hover:text-[#BF00FF]",
                            )}
                        >
                            <RiEyeOffLine className="size-14" aria-hidden="true" />
                            <span className="mt-1 text-xs font-semibold uppercase tracking-[0.2em]">Hide transaction</span>
                        </div>
                    )}
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
                                    "relative flex min-h-36 cursor-pointer items-center justify-center overflow-hidden border border-zinc-200 bg-white p-4 text-center",
                                    "transition-all duration-150 ease-out will-change-transform",
                                    isHovered ? "z-10 scale-[1.04] border-zinc-950 shadow-xl" : "hover:scale-[1.02] hover:shadow-md",
                                )}
                            >
                                <span className={cn("absolute inset-x-0 top-0 h-1", colorClasses.accent)} />
                                <span className="text-lg font-bold text-zinc-950">{category.name}</span>
                            </div>
                        );
                    })}
                </section>
            </main>

            {categoryPageCount > 1 && (
                <Button
                    type="button"
                    aria-label={categoryPage === 0 ? "Show more categories" : "Show previous categories"}
                    onClick={() => setCategoryPage((currentPage) => currentPage === 0 ? 1 : 0)}
                    className="fixed bottom-6 right-6 z-40 h-14 rounded-full bg-[#BF00FF] px-6 text-sm text-white shadow-xl shadow-[#BF00FF]/25 transition-all duration-200 hover:scale-105 hover:bg-[#9C00CF]"
                >
                    <span>{categoryPage === 0 ? "More Categories" : "Back to Categories"}</span>
                    <span className="text-xl leading-none" aria-hidden="true">{categoryPage === 0 ? "→" : "←"}</span>
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

function toTransactionDraft(transaction: Transaction): TransactionDraft {
    return {
        name: transaction.name,
        merchantName: transaction.merchantName ?? "",
        description: transaction.description ?? "",
        amount: transaction.amount.toFixed(2),
        date: transaction.date,
    };
}
