import { initializeApp } from "firebase/app";
import { getAuth, GoogleAuthProvider } from "firebase/auth";

const firebaseConfig = {
    apiKey: "AIzaSyDIwt6AiEXnPwJ8qBNvg8Aq0yyE4HR7Qqg",
    authDomain: "doughbro.firebaseapp.com",
    projectId: "doughbro",
    storageBucket: "doughbro.firebasestorage.app",
    messagingSenderId: "782689168006",
    appId: "1:782689168006:web:c55c26823ebdb8b2d97f07",
};

const app = initializeApp(firebaseConfig);

export const auth = getAuth(app);
export const googleProvider = new GoogleAuthProvider();