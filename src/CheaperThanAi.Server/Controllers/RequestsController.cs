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
            Message = "We're still working on this functionality.",
            EasterEgg = easter
        });
    }

    private static string? BuildEasterEgg(SupportRequest request)
    {
        if (request is null) return null;

        var name = (request.Name ?? string.Empty).Trim().ToLowerInvariant();
        var email = (request.Email ?? string.Empty).Trim().ToLowerInvariant();

        bool nameContains(string s) => name.Contains(s);
        bool emailContains(string s) => email.Contains(s);

        // Combined cases first
        if ((nameContains("bill") && nameContains("gates") && emailContains("ballmer")) ||
            (nameContains("ballmer") && emailContains("gates")))
        {
            return "Easter egg: Bill Gates + Steve Ballmer combo detected — iconic Microsoft duo!";
        }

        // Microsoft figures
        if (emailContains("gates") || emailContains("billgates") || emailContains("bill.gates"))
            return "Easter egg: Bill Gates' email detected — nostalgic Windows energy.";

        if (emailContains("ballmer") || nameContains("ballmer"))
            return "Easter egg: Steve Ballmer detected — Developers! Developers! Developers!";

        if (emailContains("nadella") || nameContains("nadella") || nameContains("satya") || emailContains("satya"))
            return "Easter egg: Satya Nadella detected — cloud mode activated.";

        // Other public figures
        if (emailContains("jassy") || nameContains("andy jassy") || nameContains("jassy") || nameContains("andy"))
            return "Easter egg: Andy Jassy detected — AWS vibes.";

        if (emailContains("timcook") || emailContains("cook@") || nameContains("tim cook") || nameContains("cook"))
            return "Easter egg: Tim Cook detected — keep it simple and elegant.";

        if (emailContains("bezos") || nameContains("jeff bezos") || nameContains("bezos"))
            return "Easter egg: Jeff Bezos detected — prime delivery alerted.";

        if (emailContains("jobs") || nameContains("steve jobs") || nameContains("jobs"))
            return "Easter egg: Steve Jobs detected — think different.";

        return null;
    }
}
