import { useTransition } from "react";
import type { MouseEvent } from "react";
import { Button } from "@/components/ui/button";
import { syncTransactions } from "@/services/transactionService";

interface SyncTransactionsButtonProps {
    onSyncCompleteCallback?: () => void;
}

export function SyncTransactionsButton({ onSyncCompleteCallback }: SyncTransactionsButtonProps) {
    const [isPending, startTransition] = useTransition();

    const handleSyncClick = (e: MouseEvent) => {
        e.preventDefault();
        e.stopPropagation();

        if (isPending) return;

        startTransition(async () => {
            try {
                await syncTransactions();

                if (onSyncCompleteCallback) {
                    onSyncCompleteCallback();
                }
            } catch (err) {
                console.error("Failed to synchronize transactions:", err);
            }
        });
    };

    return (
        <Button
            onClick={handleSyncClick}
            disabled={isPending}
            className="bg-blue-600 hover:bg-blue-700 text-white font-medium"
        >
            {isPending ? "Syncing Ledger..." : "Sync Transactions"}
        </Button>
    );
}
