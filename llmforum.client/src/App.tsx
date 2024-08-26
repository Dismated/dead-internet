import { useState } from 'react';
import { UserProvider } from './Hooks/UseAuth';
import { Outlet } from 'react-router';


const App = () => (
    <div className="min-h-screen bg-zinc-900">
        <UserProvider>

            <Outlet />
        </UserProvider>
    </div>)

export default App;