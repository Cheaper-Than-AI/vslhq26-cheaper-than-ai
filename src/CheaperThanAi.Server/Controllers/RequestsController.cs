using CheaperThanAi.Server.Services;
using CheaperThanAi.Shared.dto;
using CheaperThanAi.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;

namespace CheaperThanAi.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestsController : ControllerBase
{
    private readonly IAiService _aiService;

    public RequestsController(IAiService aiService)
    {
        _aiService = aiService;
    }

    [HttpPost]
    public async Task<ActionResult<SupportResponse>> Submit([FromBody] SupportRequest request)
    {
        // TODO: Use MCP/AI to decide ticket creation and subject/category in the future.
        _ = request;

        var prompt = new List<ChatMessage>
        {
            new(ChatRole.System, "You are a helpful assistant that knows a lot about IT. You are going to take any problems the user" +
            "is having and categorize, prioritize, come up with potential reasons for the problem, and potential fixes. Use the " +
            "CreateITTicket tool to create the new IT ticket for the user."),
            new(ChatRole.User, $"Hi! My name is Maria Lastname. Here is my problem: {request.Message}")
        };

        // Have AI create the ticket
        var ticket = await _aiService.GetResponse<Ticket>(prompt);

        return Ok(new SupportResponse
        {
            Message = "We're still working on this functionality."
        });
    }
}
