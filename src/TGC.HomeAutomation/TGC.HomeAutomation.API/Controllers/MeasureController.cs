using System.Net;
using Microsoft.AspNetCore.Mvc;
using TGC.HomeAutomation.API.Measure;
using TGC.HomeAutomation.Application.Abstractions;
using TGC.HomeAutomation.Application.Features.Measures.Commands.CreateDeviceMeasure;
using TGC.HomeAutomation.Infrastructure.Authentication;
using TGC.WebApi.Communication;

namespace TGC.HomeAutomation.API.Controllers;

public class MeasureController : HAControllerBase
{
	private readonly ICompositeMeasureService _measureService;
	private readonly IMediator _mediator;

	public MeasureController(ICompositeMeasureService measureService, IMediator mediator)
	{
		_measureService = measureService;
		_mediator = mediator;
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
	[ApiKeyAuthorize]
	[Route("measure/inside")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<IActionResult> Create([FromBody] MeasureRequest measureRequest)
	{
		var command = measureRequest.ToCommand();
		await _mediator.HandleCommandAsync<CreateDeviceMeasureCommand, CreateDeviceMeasureResponse>(command);
		return ApiResult.FromStatusCode(HttpStatusCode.OK).ToActionResult();
	}
}
