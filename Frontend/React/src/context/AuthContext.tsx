import { useEffect, useState } from "react";
import type { ReactNode } from "react";
import { onAuthStateChanged } from "firebase/auth";
import type { User } from "firebase/auth";
import { auth } from "@/firebase/firebase";
import { AuthContext } from "@/context/authStateContext";
import { initializeDefaultCategories } from "@/services/categoryService";

export function AuthProvider({ children }: { children: ReactNode }) {
    const [user, setUser] = useState<User | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const unsubscribe = onAuthStateChanged(auth, async (currentUser) => {
            if (currentUser) {
                try {
                    await initializeDefaultCategories();
                } catch (error) {
                    console.error("Failed to initialize default categories:", error);
                }
            }

            setUser(currentUser);
            setLoading(false);
        });

        return () => unsubscribe();
    }, []);

    return (
        <AuthContext value={{ user, loading }}>
            {children}
        </AuthContext>
    );
}
