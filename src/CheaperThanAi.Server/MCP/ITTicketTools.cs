using CheaperThanAi.Server.Data;
using CheaperThanAi.Shared.dto;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

internal class ITTicketTools
{
    private readonly TicketsDbContext _ticketetsDbContext;

    public ITTicketTools(TicketsDbContext ticketetsDbContext)
    {
        _ticketetsDbContext = ticketetsDbContext;
    }

    [McpServerTool]
    [Description("Create a new IT ticket for the user.")]
    public string CreateITTicket(
            [Description("The name of the person who submitted the ticket.")] string userName,
            [Description("The text that the user submitted as the problem that they are experiencing.")] string issueDescription,
            [Description("The priority level of the ticket.")] PriorityLevel priorityLevel,
            [Description("The category that best describes the problem the user is facing.")] string category
        )
    {
        DateTime now = DateTime.Now;
        string id = Guid.NewGuid().ToString();

        var ticket = new Ticket()
        {
            DateTime = now,
            Id = id,
            Category = category,
            PriorityLevel = priorityLevel,
            UserName = userName,
            IssueDescription = issueDescription
        };

        // Update the database with the new ticket information
        _ticketetsDbContext.Add(ticket);
        _ticketetsDbContext.SaveChanges();

        return JsonSerializer.Serialize(ticket);
    }
}