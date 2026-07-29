using CheaperThanAi.Shared.dto;
using ModelContextProtocol.Server;
using System.ComponentModel;

internal class ITTicketTools
{
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

        // Update the database with the new ticket information


        return id;
    }
}