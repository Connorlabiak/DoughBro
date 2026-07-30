import { useTransition } from "react";
import { apiFetch } from "@/lib/apiClient";
import { Button } from "@/components/ui/button";

interface SyncTransactionsButtonProps {
    onSyncCompleteCallback?: () => void;
}

export function SyncTransactionsButton({ onSyncCompleteCallback }: SyncTransactionsButtonProps) {
    const [isPending, startTransition] = useTransition();

    const handleSyncClick = (e: React.MouseEvent) => {
        e.preventDefault();
        e.stopPropagation();

        // Prevent multiple concurrent sync execution calls
        if (isPending) return;

        startTransition(async () => {
            try {
                // Hits your newly deployed .NET endpoint securely
                await apiFetch<{ success: boolean; message: string }>("/api/transactions/sync", {
                    method: "GET",
                });

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