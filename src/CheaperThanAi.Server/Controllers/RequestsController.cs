using CheaperThanAi.Server.Services;
using CheaperThanAi.Shared.dto;
using CheaperThanAi.Shared.Requests;
using CheaperThanAi.Server.Data;
using CheaperThanAi.Shared.dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using System.Linq;

namespace CheaperThanAi.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestsController : ControllerBase
{
    private readonly IAiService _aiService;
    private readonly TicketsDbContext _db;

    public RequestsController(IAiService aiService, TicketsDbContext db)
    {
        _aiService = aiService;
        _db = db;
    }

    [HttpPost]
    public async Task<ActionResult<SupportResponse>> Submit([FromBody] SupportRequest request)
    {
        // TODO: Use MCP/AI to decide ticket creation and subject/category in the future.

        var easter = BuildEasterEgg(request);

        if (easter is not null)
        {
            return Ok(new SupportResponse
            {
                Message = easter
            });
        }

        var prompt = new List<ChatMessage>
        {
            new(ChatRole.System, "You are a helpful assistant that knows a lot about IT. You are going to take any problems the user" +
            "is having and categorize, prioritize, come up with potential reasons for the problem, and potential fixes. "),
            new(ChatRole.User, $"Hi! My name is {request.Name} and my email address is {request.Email}. Here is my problem: {request.Message}")
        };

        // Have AI create the ticket (the AI may fill subject/category/priority)
        var aiResponse = await _aiService.GetResponse<Ticket>(prompt);

        if (aiResponse is null)
            return Ok(new SupportResponse { Message = "Unable to create ticket at this time." });

        // Attempt to extract the Ticket result from the AI response using reflection
        Ticket? ticket = null;
        var respType = aiResponse.GetType();
        var resultProp = respType.GetProperty("Result") ?? respType.GetProperty("Value") ?? respType.GetProperty("Response") ?? respType.GetProperty("Output");
        if (resultProp != null)
        {
            ticket = resultProp.GetValue(aiResponse) as Ticket;
        }

        if (ticket == null)
        {
            var getResultMethod = respType.GetMethod("GetResult") ?? respType.GetMethod("GetValue");
            if (getResultMethod != null)
            {
                var maybe = getResultMethod.Invoke(aiResponse, null);
                ticket = maybe as Ticket;
            }
        }

        // Fallback: try JSON round-trip from the chat response string
        if (ticket == null)
        {
            try
            {
                var json = aiResponse.ToString();
                ticket = System.Text.Json.JsonSerializer.Deserialize<Ticket>(json);
            }
            catch
            {
                ticket = null;
            }
        }

        if (ticket == null)
            return Ok(new SupportResponse { Message = "The AI produced an unexpected response and no ticket could be created." });

        // Ensure ticket has an ID and timestamp
        if (string.IsNullOrWhiteSpace(ticket.Id))
            ticket.Id = Guid.NewGuid().ToString();

        if (ticket.DateTime == default)
            ticket.DateTime = DateTime.UtcNow;

        // Ensure ticket has required/common fields filled when AI omitted them.
        // Use reflection so this code is resilient to changes in the Ticket DTO.
        var ticketType = ticket.GetType();

        // UserName
        var userNameProp = ticketType.GetProperty("UserName");
        if (userNameProp != null && userNameProp.CanWrite)
        {
            var val = userNameProp.GetValue(ticket) as string;
            if (string.IsNullOrWhiteSpace(val))
                userNameProp.SetValue(ticket, !string.IsNullOrWhiteSpace(request.Name) ? request.Name : request.Email);
        }

        // Email-like fields
        var emailProp = ticketType.GetProperty("Email") ?? ticketType.GetProperty("UserEmail");
        if (emailProp != null && emailProp.CanWrite)
        {
            var val = emailProp.GetValue(ticket) as string;
            if (string.IsNullOrWhiteSpace(val))
                emailProp.SetValue(ticket, request.Email);
        }

        // Subject
        var subjectProp = ticketType.GetProperty("Subject");
        if (subjectProp != null && subjectProp.CanWrite)
        {
            var val = subjectProp.GetValue(ticket) as string;
            if (string.IsNullOrWhiteSpace(val))
            {
                var who = !string.IsNullOrWhiteSpace(request.Name) ? request.Name : request.Email;
                var shortMsg = (request.Message ?? string.Empty).Trim();
                if (shortMsg.Length > 60) shortMsg = shortMsg.Substring(0, 57) + "...";
                subjectProp.SetValue(ticket, $"Support request from {who}: {shortMsg}");
            }
        }

        // Category
        var categoryProp = ticketType.GetProperty("Category");
        if (categoryProp != null && categoryProp.CanWrite)
        {
            var val = categoryProp.GetValue(ticket) as string;
            if (string.IsNullOrWhiteSpace(val))
                categoryProp.SetValue(ticket, "General");
        }

        // Priority
        var priorityProp = ticketType.GetProperty("Priority");
        if (priorityProp != null && priorityProp.CanWrite)
        {
            var val = priorityProp.GetValue(ticket) as string;
            if (string.IsNullOrWhiteSpace(val))
                priorityProp.SetValue(ticket, "Normal");
        }

        // Description/Message/Body
        var descProp = ticketType.GetProperty("Description") ?? ticketType.GetProperty("Message") ?? ticketType.GetProperty("Body");
        if (descProp != null && descProp.CanWrite)
        {
            var val = descProp.GetValue(ticket) as string;
            if (string.IsNullOrWhiteSpace(val))
                descProp.SetValue(ticket, request.Message);
        }

        // Status
        var statusProp = ticketType.GetProperty("Status");
        if (statusProp != null && statusProp.CanWrite)
        {
            var val = statusProp.GetValue(ticket) as string;
            if (string.IsNullOrWhiteSpace(val))
                statusProp.SetValue(ticket, "New");
        }

        // Persist ticket to the database
        await _db.Tickets.AddAsync(ticket);
        await _db.SaveChangesAsync();

        // Return a friendly message including the ticket ID for tracking
        var easterMsg = BuildEasterEgg(request);
        var baseMessage = easterMsg ?? "A ticket was created.";
        return Ok(new SupportResponse
        {
            Message = $"{baseMessage} Ticket ID: {ticket.Id}"
        });
    }

    private static string? BuildEasterEgg(SupportRequest request)
    {
        if (request is null) return null;

        var name = (request.Name ?? string.Empty).Trim().ToLowerInvariant();
        var email = (request.Email ?? string.Empty).Trim().ToLowerInvariant();

        bool nameContains(string s) => name.Contains(s);
        bool emailContains(string s) => email.Contains(s);

        // Combined cases first - creative responses
        if ((nameContains("bill") && nameContains("gates") && emailContains("ballmer")) ||
            (nameContains("ballmer") && emailContains("gates")))
        {
            return "A blast from the Microsoft past! Your request carries legendary Windows-era energy — we'll handle it with nostalgic priority.";
        }

        // Microsoft figures
        if (emailContains("gates") || emailContains("billgates") || emailContains("bill.gates"))
            return "Ah — a Gates-related contact. We'll apply rigorous, philanthropic-level attention to your issue.";

        if (emailContains("ballmer") || nameContains("ballmer"))
            return "LOUD AND CLEAR. Your submission has been received with developer-level enthusiasm and will be acted on.";

        if (emailContains("nadella") || nameContains("nadella") || nameContains("satya") || emailContains("satya"))
            return "Cloud alignment acknowledged. We're routing this to the team that keeps things intelligent and scalable.";

        // Other public figures
        if (emailContains("jassy") || nameContains("andy jassy") || nameContains("jassy") || nameContains("andy"))
            return "AWS-level attention requested — your issue will be treated like a high-availability concern.";

        if (emailContains("timcook") || emailContains("cook@") || nameContains("tim cook") || nameContains("cook"))
            return "Elegance registered. We'll handle this with careful design and simplicity.";

        if (emailContains("bezos") || nameContains("jeff bezos") || nameContains("bezos"))
            return "Prime priority noted. Your request will be expedited with maximum shipping speed.";

        if (emailContains("jobs") || nameContains("steve jobs") || nameContains("jobs"))
            return "Think different — your issue inspires creative troubleshooting. We'll approach it with imagination.";

        return null;
    }
}
