import { useState, useEffect } from 'react';
import './App.css';


function App() {
    const [aiResponse, setAiResponse] = useState('');

    useEffect(() => {
        (async () => {
            const response = await fetch('https://localhost:7201/api/AI')
            const text = await response.text()

            console.log(text, response)
            setAiResponse(text)
        })();

    }, [])

    return (
        <div>
            <h1 id="tableLabel">{aiResponse}</h1>
        </div>
    );

}

export default App;