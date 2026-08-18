import { useActionState, useState, useTransition } from "react";
import {
    signInWithEmailAndPassword,
    createUserWithEmailAndPassword,
    getAdditionalUserInfo,
    signInWithPopup,
} from "firebase/auth";
import type { AuthError } from "firebase/auth";
import { auth, googleProvider } from "@/firebase/firebase";
import { initializeDefaultCategories } from "@/services/categoryService";

import { Card, CardHeader, CardTitle, CardDescription, CardContent, CardFooter } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { Alert, AlertDescription } from "@/components/ui/alert";

import { FcGoogle } from "react-icons/fc";

interface LoginProps {
    onSuccess?: () => void;
}

export default function Login({ onSuccess }: LoginProps) {
    const [isSignUp, setIsSignUp] = useState(false);
    const [isPendingGoogle, startGoogleTransition] = useTransition();

    const getErrorMessage = (code: string) => {
        switch (code) {
            case "auth/invalid-credential":
            case "auth/user-not-found":
            case "auth/wrong-password":
                return "Invalid email or password.";
            case "auth/email-already-in-use":
                return "An account with this email already exists.";
            case "auth/weak-password":
                return "Password must be at least 6 characters.";
            default:
                return "Authentication failed. Please try again.";
        }
    };

    const [errorMessage, formAction, isPendingForm] = useActionState(
        async (_previousState: string | null, formData: FormData) => {
            const email = formData.get("email") as string;
            const password = formData.get("password") as string;

            try {
                if (isSignUp) {
                    await createUserWithEmailAndPassword(auth, email, password);
                    await initializeDefaultCategories();
                } else {
                    await signInWithEmailAndPassword(auth, email, password);
                }
                onSuccess?.();
                return null;
            } catch (err) {
                const authError = err as AuthError;
                return getErrorMessage(authError.code);
            }
        },
        null
    );

    const handleGoogleSignIn = () => {
        startGoogleTransition(async () => {
            try {
                const credential = await signInWithPopup(auth, googleProvider);
                if (getAdditionalUserInfo(credential)?.isNewUser) {
                    await initializeDefaultCategories();
                }
                onSuccess?.();
            } catch (err) {
                const authError = err as AuthError;
                if (authError.code !== "auth/popup-closed-by-user") {
                    console.error("Google sign-in failed", authError);
                }
            }
        });
    };

    const isPending = isPendingForm || isPendingGoogle;

    return (
        <div className="flex min-h-screen items-center justify-center p-4 bg-background">
            <Card className="w-full max-w-md shadow-lg">
                <CardHeader className="space-y-1 text-center">
                    <CardTitle className="text-2xl font-bold">
                        {isSignUp ? "Create an account" : "Welcome back"}
                    </CardTitle>
                    <CardDescription>
                        {isSignUp
                            ? "Enter your email below to create your account"
                            : "Enter your credentials to access your account"}
                    </CardDescription>
                </CardHeader>

                <CardContent className="space-y-4">
                    {errorMessage && (
                        <Alert variant="destructive">
                            <AlertDescription>{errorMessage}</AlertDescription>
                        </Alert>
                    )}

                    {/* React 19 Form Action — No onSubmit, no preventDefault, native FormData */}
                    <form action={formAction} className="space-y-3">
                        <div className="space-y-1">
                            <Input
                                name="email"
                                type="email"
                                placeholder="name@example.com"
                                required
                            />
                        </div>
                        <div className="space-y-1">
                            <Input
                                name="password"
                                type="password"
                                placeholder="Password"
                                required
                            />
                        </div>
                        <Button type="submit" className="w-full" disabled={isPending}>
                            {isPendingForm ? "Processing..." : isSignUp ? "Sign Up" : "Sign In"}
                        </Button>
                    </form>

                    <div className="relative my-4">
                        <div className="absolute inset-0 flex items-center">
                            <span className="w-full border-t" />
                        </div>
                        <div className="relative flex justify-center text-xs uppercase">
                            <span className="bg-card px-2 text-muted-foreground">
                                Or continue with
                            </span>
                        </div>
                    </div>

                    <Button
                        type="button"
                        variant="outline"
                        className="w-full"
                        onClick={handleGoogleSignIn}
                        disabled={isPending}
                    >
                        <FcGoogle className="mr-2 h-4 w-4" />
                        {isPendingGoogle ? "Connecting..." : "Google"}
                    </Button>
                </CardContent>

                <CardFooter className="justify-center">
                    <p className="text-sm text-muted-foreground">
                        {isSignUp ? "Already have an account?" : "Don't have an account?"}{" "}
                        <button
                            type="button"
                            className="text-primary underline font-medium cursor-pointer"
                            onClick={() => setIsSignUp(!isSignUp)}
                        >
                            {isSignUp ? "Sign In" : "Sign Up"}
                        </button>
                    </p>
                </CardFooter>
            </Card>
        </div>
    );
}
