using Microsoft.AspNetCore.Mvc;
using TGC.HomeAutomation.API.Event;

namespace TGC.HomeAutomation.API.Controllers;

public class EventController : HAControllerBase
{
	[HttpGet("events")]
	[ProducesResponseType(typeof(IEnumerable<EventResponse>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public Task<IEnumerable<EventResponse>> GetAllEvents()
	{
		return Task.FromResult(new List<EventResponse>().AsEnumerable());
	}
}
