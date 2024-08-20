import { useState, useEffect } from 'react';
import './App.css';


function App() {
    const [userText, setUserText] = useState('start');
    const [aiResponse, setAiResponse] = useState('');

    useEffect(() => {
        (async () => {
            const response = await fetch('https://localhost:7201/api/AI', {
                method: 'POST', // Specify the request method
                headers: {
                    'Content-Type': 'application/json', // Specify the content type
                },
                body: JSON.stringify({ Text: userText })
            })
            const json = await response.json()

            console.log(json, response)
            setAiResponse(json.result)
        })();

    }, [userText])

    return (
        <div>
            <input type="text" value={userText} onChange={(e) => setUserText(e.target.value)} />
            <p >{aiResponse}</p>
        </div>
    );

}

export default App;