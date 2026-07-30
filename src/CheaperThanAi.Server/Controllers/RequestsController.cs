using CheaperThanAi.Shared.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CheaperThanAi.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestsController : ControllerBase
{
    [HttpPost]
    public ActionResult<SupportResponse> Submit([FromBody] SupportRequest request)
    {
        // TODO: Use MCP/AI to decide ticket creation and subject/category in the future.
        _ = request;

        return Ok(new SupportResponse
        {
            Message = "We're still working on this functionality."
        });
    }
}
