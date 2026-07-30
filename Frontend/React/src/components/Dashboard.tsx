import React from "react";
import { Button } from "@/components/ui/button";
import { signOut } from "firebase/auth";
import { auth } from "@/firebase/firebase";
import { PlaidLinkButton } from "@/components/PlaidLinkButton";
import { SyncTransactionsButton } from "./SyncTransactionsButton";

export default function Dashboard() {
    return (
        <div className="flex h-screen items-center justify-center">
            <h1>{"Dashboard! "}</h1>
            <Button onClick={() => logout()}>Logout</Button>
            <PlaidLinkButton></PlaidLinkButton>
            <SyncTransactionsButton></SyncTransactionsButton>
        </div>
    );
}

async function logout() {
    await signOut(auth);
    //Add more soon.
}