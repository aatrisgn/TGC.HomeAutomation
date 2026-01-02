using Microsoft.AspNetCore.Mvc;

namespace TGC.HomeAutomation.API.Controllers;

[ApiController]
[Route("api")]
[Produces("application/json", [])]
[Consumes("application/json", [])]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
[ProducesResponseType(StatusCodes.Status403Forbidden)]
[ProducesResponseType(StatusCodes.Status500InternalServerError)]
public abstract class HAControllerBase : ControllerBase
{
}
