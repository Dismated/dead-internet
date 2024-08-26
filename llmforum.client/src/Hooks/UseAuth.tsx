import { createContext, useContext, useEffect, useState } from "react"
import { UserProfile } from "../Types/User"
import { loginAPI, registerAPI } from "../Services/AuthService"
import { useNavigate } from "react-router"

type UserContextType = {
    user: UserProfile | null
    token: string | null
    registerUser: (email: string, username: string, password: string) => void
    loginUser: (username: string, password: string) => void
    logoutUser: () => void
    isLoggedIn: () => boolean
}

type Props = {
    children: React.ReactNode
}



const UserContext = createContext<UserContextType>({} as UserContextType)

export const UserProvider = ({ children }: Props) => {
    const navigate = useNavigate()
    const [token, setToken] = useState<string | null>(null)
    const [user, setUser] = useState<UserProfile | null>(null)
    const [isReady, setIsReady] = useState(false)

    useEffect(() => {
        const user = localStorage.getItem("user")
        const token = localStorage.getItem("token")
        if (user && token) {
            setUser(JSON.parse(user))
            setToken(token)
        }
        setIsReady(true)
    })
    const registerUser = async (email: string, username: string, password: string) => {
        try {
            const response = await registerAPI(email, username, password)
            const headers = new Headers({
                'Authorization': `Bearer ${token}`
            });
            if (response) {
                localStorage.setItem("token", response?.data.token)
                const userObj = {
                    username: response?.data.userName,
                    email: response?.data.email
                }
                localStorage.setItem("user", JSON.stringify(userObj))
                setToken(response?.data.token!)
                setUser(userObj)
                toast.success("Registration Successful")
                navigate("/search")
            }
        } catch (error) {
            toast.error("Registration Failed")
        }
    }
    const loginUser = async (username: string, password: string) => {
        try {
            const response = await loginAPI(username, password)
            if (response) {
                localStorage.setItem("token", response?.data.token)
                const userObj = {
                    username: response?.data.userName,
                    email: response?.data.email
                }
                localStorage.setItem("user", JSON.stringify(userObj))
                setToken(response?.data.token!)
                setUser(userObj)
                toast.success("Registration Successful")
                navigate("/search")
            }
        } catch (error) {
            toast.error("Registeration Failed")
        }
    }

    const isLoggedIn = () => {
        return !!user
    }
    const logoutUser = () => {
        localStorage.removeItem("token")
        localStorage.removeItem("user")
        setToken(null)
        setUser(null)
        navigate("/")
    }
    return (
        <UserContext.Provider
            value={{
                user,
                token,
                registerUser,
                loginUser,
                logoutUser,
                isLoggedIn
            }}
        >
            {isReady ? children : null}
        </UserContext.Provider>
    )
}

export const useAuth = () => {
    return useContext(UserContext)
}