import { BrowserRouter, Routes, Route, useNavigate } from "react-router-dom";
import SignIn from "@/components/Login";
import { AuthProvider } from "@/context/AuthContext";
import { ProtectedRoute, PublicOnlyRoute } from "@/components/RouteGuards";
import Dashboard from "@/components/Dashboard";

function LoginPage() {
    const navigate = useNavigate();

    return (
        <SignIn onSuccess={() => navigate("/dashboard")} />
    );
}

export default function App() {
    return (
        <AuthProvider>
            <BrowserRouter>
                <Routes>
                    <Route element={<PublicOnlyRoute />}> 
                        <Route path="/login" element={<LoginPage />} />
                    </Route>

                    <Route element={<ProtectedRoute />}>
                        <Route path="/dashboard" element={<Dashboard />} />
                    </Route>

                    <Route path="/" element={<Dashboard />} />

                    <Route path="*" element={<Dashboard />} />
                </Routes>
            </BrowserRouter>
        </AuthProvider>
    );
}