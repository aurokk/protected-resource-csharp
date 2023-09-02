using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("values")]
public class ValuesController : ControllerBase
{
    [HttpGet]
    [Route("")]
    [Authorize("read")]
    public IActionResult GetValues(CancellationToken ct)
    {
        return Ok();
    }
}