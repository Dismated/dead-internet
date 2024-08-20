using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;

[ApiController]
[Route("api/[controller]")]
public class AIController : ControllerBase
{
    public AIController() { }

    [HttpGet(Name = "GetAI")]
    public async Task<IActionResult> Get(Kernel kernel)
    {
        var result = await kernel.InvokePromptAsync<string>("hello");
        return Ok(result);
    }
}