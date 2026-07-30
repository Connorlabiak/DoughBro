import { useState, useTransition, useEffect } from "react"; // Added useEffect
import { usePlaidLink } from "react-plaid-link";
import { apiFetch } from "@/lib/apiClient";
import { Button } from "@/components/ui/button";

interface PlaidLinkButtonProps {
    onSuccessCallback?: () => void;
}

export function PlaidLinkButton({ onSuccessCallback }: PlaidLinkButtonProps) {
    const [linkToken, setLinkToken] = useState<string | null>(null);
    const [isPending, startTransition] = useTransition();
    const [hasOpened, setHasOpened] = useState(false); // Track if we already triggered open()

    // Step 1: Get link_token from ASP.NET backend
    const fetchLinkToken = async () => {
        // Reset tracker tracking state if requesting a fresh token
        setHasOpened(false);

        startTransition(async () => {
            try {
                const data = await apiFetch<{ linkToken: string }>("/api/plaid/create-link-token", {
                    method: "POST",
                });
                setLinkToken(data.linkToken);
            } catch (err) {
                console.error("Error creating Plaid Link token:", err);
            }
        });
    };

    // Step 2: Configure Plaid Link Hook
    const { open, ready } = usePlaidLink({
        token: linkToken,
        onSuccess: async (public_token, metadata) => {
            startTransition(async () => {
                try {
                    await apiFetch("/api/plaid/exchange-public-token", {
                        method: "POST",
                        body: JSON.stringify({
                            publicToken: public_token,
                            institutionName: metadata.institution?.name ?? "Unknown Bank",
                        }),
                    });
                    if (onSuccessCallback) {
                        onSuccessCallback();
                    }
                } catch (err) {
                    console.error("Failed to exchange public token:", err);
                } finally {
                    // Clean up state after exchange complete
                    setLinkToken(null);
                    setHasOpened(false);
                }
            });
        },
        onExit: () => {
            // If user closes modal manually, reset states so they can click again
            setLinkToken(null);
            setHasOpened(false);
        }
    });

    // Step 3: Automatically open Plaid Link as soon as the hook is ready
    useEffect(() => {
        if (ready && linkToken && !hasOpened) {
            setHasOpened(true); // Prevent multi-triggers
            open();
        }
    }, [ready, linkToken, hasOpened, open]);

    const handleClick = (e: React.MouseEvent) => {
        e.preventDefault();
        e.stopPropagation(); // Stop click bubbles

        // Only fetch if a request isn't already active and we don't have a token
        if (!linkToken && !isPending) {
            fetchLinkToken();
        }
    };

    return (
        <Button
            onClick={handleClick}
            disabled={isPending || (!!linkToken && !ready)} // Keep disabled during loading transitions
            className="bg-emerald-600 hover:bg-emerald-700 text-white font-medium"
        >
            {isPending ? "Connecting..." : "Link Bank Account"}
        </Button>
    );
}