using Microsoft.AspNetCore.Mvc;
using TGC.HomeAutomation.API.Temperature;

namespace TGC.HomeAutomation.API.Controllers;

public class TemperatureController : HAControllerBase
{
	private readonly ITemperatureService _temperatureService;

	public TemperatureController(ITemperatureService temperatureService)
	{
		_temperatureService = temperatureService;
	}

	[HttpGet]
	[Route("temperatures/inside/current")]
	[ProducesResponseType(typeof(TemperatureResponse), StatusCodes.Status200OK)]
	public async Task<TemperatureResponse> GetCurrentInside()
	{
		return await _temperatureService.GetCurrentInside();
	}

	[HttpGet]
	[Route("temperatures/outside/current")]
	[ProducesResponseType(typeof(TemperatureResponse), StatusCodes.Status200OK)]
	public async Task<TemperatureResponse> GetCurrentOutside()
	{
		return await _temperatureService.GetCurrentOutside();
	}

	[HttpGet]
	[Route("temperatures/inside/{startDate}/{endDate}")]
	[ProducesResponseType(typeof(IEnumerable<TemperatureResponse>), StatusCodes.Status200OK)]
	public async Task<IEnumerable<TemperatureResponse>> GetCurrentOutside(DateTime startDate, DateTime endDate)
	{
		return await _temperatureService.GetAccumulatedByHour(startDate, endDate);
	}

	[HttpPost]
	[Route("temperatures/inside")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task Create([FromBody] TemperatureRequest request)
	{
		await _temperatureService.AddRead(request);
	}
}
