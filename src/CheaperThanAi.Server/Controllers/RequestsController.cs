using CheaperThanAi.Shared.Requests;
using CheaperThanAi.Server.Data;
using CheaperThanAi.Shared.dto;
using Microsoft.AspNetCore.Mvc;

namespace CheaperThanAi.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestsController : ControllerBase
{
    private readonly TicketsDbContext _db;

    public RequestsController(TicketsDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<ActionResult<SupportResponse>> Submit([FromBody] SupportRequest request)
    {
        // TODO: Replace the following default behavior with AI-driven logic that
        // decides whether a ticket is needed and generates subject/category/priority.
        // For now always create a ticket with sensible defaults when AI is not available.

        // Choose a display name: prefer Name, fall back to UserName
        var displayName = string.IsNullOrWhiteSpace(request.Name) ? request.UserName : request.Name;

        // Sensible defaults when AI cannot determine better values
        var category = "General";
        var priority = PriorityLevel.Low;
        var issueDescription = request.Message ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            issueDescription += $"\n\nContact Email: {request.Email}";
        }

        var ticket = new Ticket
        {
            Id = Guid.NewGuid().ToString(),
            DateTime = DateTime.UtcNow,
            UserName = displayName,
            IssueDescription = issueDescription,
            PriorityLevel = priority,
            Category = category
        };

        await _db.Tickets.AddAsync(ticket);
        await _db.SaveChangesAsync();

        return Ok(new SupportResponse
        {
            Message = $"A ticket was created (ID: {ticket.Id}). Our team will follow up at {request.Email ?? "the contact on file"}."
        });
    }
}
