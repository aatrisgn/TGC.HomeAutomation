using Microsoft.AspNetCore.Mvc;
using TGC.HomeAutomation.API.Authentication;
using TGC.HomeAutomation.API.Measure;

namespace TGC.HomeAutomation.API.Controllers;

public class MeasureController : HAControllerBase
{
	private readonly ICompositeMeasureService _measureService;

	public MeasureController(ICompositeMeasureService measureService)
	{
		_measureService = measureService;
	}

	[HttpGet]
	[Route("measure/{measureType}/inside/current")]
	[ProducesResponseType(typeof(MeasureResponse), StatusCodes.Status200OK)]
	public async Task<MeasureResponse> GetCurrentInside(string measureType)
	{
		return await _measureService.GetCurrentMeasureInside(measureType);
	}

	[HttpGet]
	[Route("measure/{measureType}/outside/current")]
	[ProducesResponseType(typeof(MeasureResponse), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetCurrentOutside(string measureType)
	{
		var apiResult = await _measureService.GetCurrentOutside(measureType);
		return apiResult.ToActionResult();
	}

	[HttpGet]
	[Route("measure/{measureType}/inside/{startDate}/{endDate}")]
	[ProducesResponseType(typeof(MeasureRangeResponse), StatusCodes.Status200OK)]
	public async Task<MeasureRangeResponse> GetMeasuresByDate(string measureType, DateTime startDate, DateTime endDate)
	{
		return await _measureService.GetAverageBy10Minutes(measureType, startDate, endDate);
	}

	[HttpGet]
	[Route("measure/{deviceId}/latestactivity")]
	[ProducesResponseType(typeof(MeasureRangeResponse), StatusCodes.Status200OK)]
	public async Task<MeasureRangeResponse> GetLatestActivityByDeviceId(Guid deviceId)
	{
		return await _measureService.GetLatestActivityByDeviceId(deviceId);
	}

	[HttpGet]
	[Route("measure/{deviceId}/{startDate}/{endDate}")]
	[ProducesResponseType(typeof(DeviceOrderedMeasureRangeResponse), StatusCodes.Status200OK)]
	public async Task<DeviceOrderedMeasureRangeResponse> GetMeasuresByDeviceIdAndDate(Guid deviceId, DateTime startDate, DateTime endDate)
	{
		return await _measureService.GetByDeviceId(deviceId, startDate, endDate);
	}

	[HttpGet]
	[Route("measure/{deviceId}/{measureType}/{startDate}/{endDate}")]
	[ProducesResponseType(typeof(DeviceOrderedMeasureRangeResponse), StatusCodes.Status200OK)]
	public async Task<DeviceOrderedMeasureRangeResponse> GetMeasuresByDeviceIdMeasureTypeAndDate(Guid deviceId, string measureType, DateTime startDate, DateTime endDate)
	{
		return await _measureService.GetSpecificMeasuresByDeviceIdForPeriod(measureType, deviceId, startDate, endDate);
	}

	[HttpPost]
	[APIKeyAuthorize]
	[Route("measure/inside")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task Create([FromBody] MeasureRequest request)
	{
		await _measureService.AddRead(request);
	}
}
