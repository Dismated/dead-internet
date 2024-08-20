using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;

[ApiController]
[Route("api/[controller]")]
public class AIController : ControllerBase
{
    private readonly Kernel _kernel;
    public AIController(Kernel kernel) { _kernel = kernel; }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AiPrompt prompt)
    {
        var result = await _kernel.InvokePromptAsync<string>(prompt.Text);
        return Ok(new { Message = "hello", Result = result });
    }

    public class AiPrompt
    {
        public string Text { get; set; }
    }
}
