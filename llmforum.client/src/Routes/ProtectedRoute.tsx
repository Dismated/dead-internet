import { ReactNode } from "react"
import { useAuth } from "../Hooks/UseAuth"
import { useLocation, Navigate } from "react-router"

type Props = {
    children: ReactNode
}

const ProtectedRoute = ({ children }: Props) => {
    const location = useLocation()
    const { isLoggedIn } = useAuth()
    return isLoggedIn() ? <>{children}</> : <Navigate to="/login" state={{ from: location }} replace />
}

export default ProtectedRoute