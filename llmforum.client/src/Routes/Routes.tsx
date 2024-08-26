import { createBrowserRouter } from "react-router-dom";
import App from "../App";
import HomePage from "../Pages/HomePage";
import PostPage from "../Pages/PostPage";
import LoginPage from "../Pages/LoginPage";
import RegisterPage from "../Pages/RegisterPage";
import ProtectedRoute from "./ProtectedRoute";

export const router = createBrowserRouter([{
    path: "/",
    element: <App />,
    children: [{
        path: "", element: <ProtectedRoute><HomePage /></ProtectedRoute>
    },
    { path: "post", element: <ProtectedRoute><PostPage /></ProtectedRoute> },
    { path: "login", element: <LoginPage /> },
    { path: "register", element: <RegisterPage /> }]
}])