import { UserProfileToken } from "../Types/User";

const api = 'http://localhost:8080/api/';

export const loginAPI = async (username: string, password: string) => {
    try {
        const data = await fetch(`${api}account/login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ username, password })
        });
        return await data.json() as UserProfileToken;
    }
    catch (error) {
        return { error };
    }
}

export const registerAPI = async (username: string, password: string, email: string) => {
    try {
        const data = await fetch(`${api}account/register`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ username, password, email })
        });
        return await data.json() as UserProfileToken;
    }
    catch (error) {
        return { error };
    }
}
