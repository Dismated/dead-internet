const PostPage = () => {
    const [userText, setUserText] = useState('hello');
    const [aiResponse, setAiResponse] = useState([]);

    const handleKeyDown = async (event: { key: string; }) => {
        if (event.key === 'Enter') {
            const response = await fetch('https://localhost:7201/api/Comment', {
                method: 'POST', // Specify the request method
                headers: {
                    'Content-Type': 'application/json', // Specify the content type
                },
                body: JSON.stringify({ UserPrompt: userText, PostId: 1 })
            })
            const json = await response.json()
            setAiResponse(json.comments)
        }
    }
    return (
        <div>
            <input type="text" value={userText} onChange={(e) => setUserText(e.target.value)} onKeyDown={handleKeyDown} />
            {aiResponse.map((x: { content: string; }, i) => <div key={i}>{"----".repeat(i) + ' '}{x.content}</div>)}
        </div>
    )
}
export default PostPage