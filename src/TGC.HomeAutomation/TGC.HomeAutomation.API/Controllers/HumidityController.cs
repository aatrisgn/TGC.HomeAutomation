using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TGC.HomeAutomation.API.Humidity;
using TGC.HomeAutomation.API.Temperature;

namespace TGC.HomeAutomation.API.Controllers;

public class HumidityController : HAControllerBase
{
	private readonly IHumidityService _humidityService;
	public HumidityController(IHumidityService humidityService)
	{
		_humidityService = humidityService;
	}

	[HttpGet]
	[Route("humidities/inside/current")]
	[ProducesResponseType(typeof(HumidityResponse), StatusCodes.Status200OK)]
	public async Task<HumidityResponse> GetCurrentInside()
	{
		return await _humidityService.GetCurrentInside();
	}

	[HttpGet]
	[Route("humidities/outside/current")]
	[ProducesResponseType(typeof(HumidityResponse), StatusCodes.Status200OK)]
	public async Task<HumidityResponse> GetCurrentOutside()
	{
		return await _humidityService.GetCurrentOutside();
	}

	[HttpGet]
	[Route("humidities/inside/{startDate}/{endDate}")]
	[ProducesResponseType(typeof(IEnumerable<HumidityResponse>), StatusCodes.Status200OK)]
	public async Task<IEnumerable<HumidityResponse>> GetCurrentOutside(DateTime startDate, DateTime endDate)
	{
		return await _humidityService.GetAverageBy10Minutes(startDate, endDate);
	}

	[HttpPost]
	[Authorize]
	[Route("humidities/inside")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task Create([FromBody] HumidityRequest request)
	{
		await _humidityService.AddRead(request);
	}
}
