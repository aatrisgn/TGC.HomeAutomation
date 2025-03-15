using Microsoft.AspNetCore.Mvc;
using TGC.HomeAutomation.API.Event;

namespace TGC.HomeAutomation.API.Controllers;

public class EventController : HAControllerBase
{
	[HttpPost]
	[Route("events")]
	[ProducesResponseType(typeof(EventResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public Task<EventResponse> CreateNewDevice([FromBody] EventRequest eventRequest)
	{
		var eventResponse = new EventResponse { Id = Guid.NewGuid() };
		return Task.FromResult(eventResponse);
	}
}
