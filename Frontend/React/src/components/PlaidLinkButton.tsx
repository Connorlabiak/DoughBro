import { useEffect, useState, useTransition } from "react";
import type { MouseEvent } from "react";
import { usePlaidLink } from "react-plaid-link";
import { Button } from "@/components/ui/button";
import { createLinkToken, exchangePublicToken } from "@/services/plaidService";

interface PlaidLinkButtonProps {
    onSuccessCallback?: () => void;
}

export function PlaidLinkButton({ onSuccessCallback }: PlaidLinkButtonProps) {
    const [linkToken, setLinkToken] = useState<string | null>(null);
    const [isPending, startTransition] = useTransition();
    const [hasOpened, setHasOpened] = useState(false);

    const fetchLinkToken = async () => {
        setHasOpened(false);

        startTransition(async () => {
            try {
                const data = await createLinkToken();
                setLinkToken(data.linkToken);
            } catch (err) {
                console.error("Error creating Plaid Link token:", err);
            }
        });
    };

    const { open, ready } = usePlaidLink({
        token: linkToken,
        onSuccess: async (publicToken, metadata) => {
            startTransition(async () => {
                try {
                    if (!publicToken) {
                        throw new Error("Plaid returned an empty public token.");
                    }

                    await exchangePublicToken({
                        publicToken,
                        institutionName: metadata.institution?.name || "Unknown Bank",
                    });
                    if (onSuccessCallback) {
                        onSuccessCallback();
                    }
                } catch (err) {
                    console.error("Failed to exchange public token:", err);
                } finally {
                    setLinkToken(null);
                    setHasOpened(false);
                }
            });
        },
        onExit: () => {
            setLinkToken(null);
            setHasOpened(false);
        }
    });

    useEffect(() => {
        if (ready && linkToken && !hasOpened) {
            setHasOpened(true);
            open();
        }
    }, [ready, linkToken, hasOpened, open]);

    const handleClick = (e: MouseEvent) => {
        e.preventDefault();
        e.stopPropagation();

        if (!linkToken && !isPending) {
            fetchLinkToken();
        }
    };

    return (
        <Button
            onClick={handleClick}
            disabled={isPending || (!!linkToken && !ready)}
            variant="outline"
            className="border-[#BF00FF] bg-white font-medium text-[#BF00FF] hover:bg-[#BF00FF]/10 hover:text-[#BF00FF]"
        >
            {isPending ? "Connecting..." : "Link Bank Account"}
        </Button>
    );
}
