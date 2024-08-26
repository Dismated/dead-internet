import { yupResolver } from '@hookform/resolvers/yup'
import * as Yup from 'yup' //Import from Yup
import { useAuth } from '../Hooks/UseAuth'
import { useForm } from 'react-hook-form'

type RegisterFormInputs = {
    username: string,
    password: string,
    email: string
}

const validation = Yup.object().shape({
    username: Yup.string().required('Username is required'),
    password: Yup.string().required('Password is required'),
    email: Yup.string().required('Email is required')
})

const RegisterPage = () => {
    const { registerUser } = useAuth()
    const { register, handleSubmit, formState: { errors } } = useForm<RegisterFormInputs>({
        resolver: yupResolver(validation),
    })
    const handleLogin = (form: RegisterFormInputs) => {
        registerUser(form.username, form.password, form.email)
    }
    return <section >
        <div className="flex flex-col items-center justify-center px-6 py-8 mx-auto md:h-screen lg:py-0">
            <div className="w-full bg-white rounded-lg shadow dark:border md:mb-20 sm:max-w-md xl:p-0 dark:bg-zinc-800 dark:border-zinc-700">
                <div className="p-6 space-y-4 md:space-y-6 sm:p-8">
                    <h1 className="text-xl font-bold leading-tight tracking-tight text-gray-900 md:text-2xl dark:text-white">
                        Sign in to your account
                    </h1>
                    <form className="space-y-4 md:space-y-6" onSubmit={handleSubmit(handleLogin)}>
                        <div>
                            <label
                                htmlFor="email"
                                className="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                            >
                                Username
                            </label>
                            <input
                                type="text"
                                id="username"
                                className="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-zinc-700 dark:border-zinc-600 dark:placeholder-zinc-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                                placeholder="Username"
                                {...register("username")}
                            />
                        </div>
                        {errors.username ? <p>{errors.username.message}</p> : ""}
                        <div>
                            <label
                                htmlFor="password"
                                className="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                            >
                                Password
                            </label>
                            <input
                                type="password"
                                id="password"
                                placeholder="***********"
                                className="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-zinc-700 dark:border-zinc-600 dark:placeholder-zinc-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                                {...register("password")} />
                            {errors.password ? <p>{errors.password.message}</p> : ""}
                        </div>
                        <div>
                            <label
                                htmlFor="email"
                                className="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                            >
                                Password
                            </label>
                            <input
                                type="text"
                                id="email"
                                placeholder="email@email.com"
                                className="bg-gray-50 border border-gray-300 text-gray-900 sm:text-sm rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full p-2.5 dark:bg-zinc-700 dark:border-zinc-600 dark:placeholder-zinc-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                                {...register("email")} />
                            {errors.email ? <p>{errors.email.message}</p> : ""}
                        </div>

                        <button
                            type="submit"
                            className="w-full text-white bg-primary-600 hover:bg-primary-700 focus:ring-4 focus:outline-none focus:ring-primary-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-primary-600 dark:hover:bg-primary-700 dark:focus:ring-primary-800"
                        >
                            Sign in
                        </button>
                        <p className="text-sm font-light text-gray-500 dark:text-zinc-400">
                            Don't have an account yet?{" "}
                            <a
                                href="#"
                                className="font-medium text-primary-600 hover:underline dark:text-primary-500"
                            >
                                Sign up
                            </a>
                        </p>
                    </form>
                </div>
            </div>
        </div>
    </section>
}
export default RegisterPage