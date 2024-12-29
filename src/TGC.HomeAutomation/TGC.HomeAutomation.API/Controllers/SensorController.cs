using Microsoft.AspNetCore.Mvc;

namespace TGC.HomeAutomation.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SensorController : ControllerBase
{
	[HttpGet]
	public Task<string> Get()
	{
		return Task.FromResult("OK");
	}
}
