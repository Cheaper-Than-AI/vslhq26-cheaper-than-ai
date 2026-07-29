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
        _ = request;

        return Ok(new SupportResponse
        {
            Message = "We're still working on this functionality."
        });
    }
}
