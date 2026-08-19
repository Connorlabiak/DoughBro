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
            variant="outline"
            className="border-[#BF00FF] bg-white font-medium text-[#BF00FF] hover:bg-[#BF00FF]/10 hover:text-[#BF00FF]"
        >
            {isPending ? "Syncing Ledger..." : "Sync Transactions"}
        </Button>
    );
}
