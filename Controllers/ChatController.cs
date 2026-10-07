using CopilotHybrid.Models;
using CopilotHybrid.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IAiRouterService _aiRouterService;

    public ChatController(IAiRouterService aiRouterService)
    {
        _aiRouterService = aiRouterService;
    }

    [HttpPost("ask")]
    public async Task<IActionResult> AskCopilot([FromBody] ChatRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Prompt))
        {
            return BadRequest(new { error = "O prompt não pode ser vazio." });
        }

        var response = await _aiRouterService.RouteAndProcessAsync(request.Prompt);
        return Ok(new { result = response });
    }
}