using CheaperThanAi.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace CheaperThanAi.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestsController : ControllerBase
{
    [HttpPost]
    public ActionResult<SupportResponse> Submit([FromBody] SupportRequest request)
    {
        // TODO: Use MCP/AI to decide ticket creation and subject/category in the future.

        var easter = BuildEasterEgg(request);

        return Ok(new SupportResponse
        {
            Message = easter ?? "We're still working on this functionality."
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
