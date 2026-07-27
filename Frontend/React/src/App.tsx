import { BrowserRouter, Routes, Route, useNavigate } from "react-router-dom";
import SignIn from "@/components/Login";

function LoginPage() {
    const navigate = useNavigate();

    return (
        <SignIn onSuccess={() => navigate("/dashboard")} />
    );
}

export default function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/login" element={<LoginPage />} />
                {/* Your other routes */}
            </Routes>
        </BrowserRouter>
    );
}