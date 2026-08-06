import { BrowserRouter, Routes, Route, useNavigate, Navigate } from "react-router-dom";
import SignIn from "@/components/Login";
import { AuthProvider } from "@/context/AuthContext";
import { ProtectedRoute, PublicOnlyRoute } from "@/components/RouteGuards";
import Dashboard from "@/components/Dashboard";
import CategoriesPage from "@/components/categories/CategoriesPage";

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
                        <Route path="/categories" element={<CategoriesPage />} />
                    </Route>

                    <Route path="/" element={<Navigate to="/dashboard" replace />} />

                    <Route path="*" element={<Navigate to="/dashboard" replace />} />
                </Routes>
            </BrowserRouter>
        </AuthProvider>
    );
}
