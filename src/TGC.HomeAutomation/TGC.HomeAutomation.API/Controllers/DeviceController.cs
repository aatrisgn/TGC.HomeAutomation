using Microsoft.AspNetCore.Mvc;
using TGC.HomeAutomation.API.Contracts.Device;
using TGC.HomeAutomation.API.Device;
using TGC.HomeAutomation.Application.Abstractions;
using TGC.HomeAutomation.Application.Features.Devices.Commands.DeleteDevice;
using TGC.HomeAutomation.Application.Features.Devices.Commands.UpsertApiKeyForDevice;
using TGC.HomeAutomation.Application.Features.Devices.Queries.GetAllDevices;
using TGC.HomeAutomation.Application.Features.Devices.Queries.GetDeviceById;
using TGC.WebApi.Communication;

namespace TGC.HomeAutomation.API.Controllers;

public class DeviceController : HAControllerBase
{
	private readonly IDeviceService _deviceService;
	private readonly IMediator _mediator;

	public DeviceController(IDeviceService deviceService, IMediator mediator)
	{
		_deviceService = deviceService;
		_mediator = mediator;
	}

	[HttpGet]
	[Route("devices")]
	[ProducesResponseType(typeof(DeviceCollectionResponse), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetAllDevices()
	{
		var query = GetAllDevicesQuery.Empty();
		var result = await _mediator.HandleQueryAsync<GetAllDevicesQuery, GetAllDevicesResponse>(query);

		var response = DeviceCollectionResponse.FromQueryResponse(result);

		return ApiResult<DeviceCollectionResponse>.AsOk(response).ToActionResult();
	}

	[HttpGet]
	[Route("devices/{id:guid}")]
	[ProducesResponseType(typeof(DeviceResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetSingleDeviceById(Guid id)
	{
		var query = new GetDeviceByIdQuery(id);
		var result = await _mediator.HandleQueryAsync<GetDeviceByIdQuery, GetDeviceByIdResponse>(query);
		var response = DeviceResponse.FromQueryResponse(result);

		return ApiResult<DeviceResponse>.AsOk(response).ToActionResult();
	}

	[HttpGet]
	[Route("devices/{id:guid}/healthcheck")]
	[ProducesResponseType(typeof(DeviceHealthCheckResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetHealthCheckById(Guid id)
	{
		var result = await _deviceService.CheckDeviceHealthAsync(id);
		return result.ToActionResult();
	}

	[HttpGet]
	[Route("devices/{id:guid}/measuretypes")]
	[ProducesResponseType(typeof(DeviceMeasureTypesResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetAvailableMeasuresByDeviceId(Guid id)
	{
		var deviceMeasureTypesResponse = await _deviceService.GetAvailableMeasureTypesByDeviceId(id);
		return deviceMeasureTypesResponse != null ? Ok(deviceMeasureTypesResponse) : NotFound();
	}

	[HttpPost]
	[Route("devices")]
	[ProducesResponseType(typeof(DeviceResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<DeviceResponse> CreateNewDevice([FromBody] DeviceRequest deviceRequest)
	{
		var newDevice = await _deviceService.CreateAsync(deviceRequest);
		return newDevice;
	}

	[HttpPut]
	[Route("devices/{id:guid}/apikey")]
	[ProducesResponseType(typeof(ApiKeyResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ApiKeyResponse> UpdateApiKey(Guid id, [FromBody] ApiKeyRequest apiKeyRequest)
	{
		var command = apiKeyRequest.ToCommand(id);

		var result = await _mediator.HandleCommandAsync<UpsertApiKeyForDeviceCommand, UpsertApiKeyForDeviceResponse>(command);

		var response = ApiKeyResponse.FromCommand(result);

		return response;
	}

	[HttpPut]
	[Route("devices/{id:guid}")]
	[ProducesResponseType(typeof(DeviceResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public Task<IActionResult> UpdateSingleDevice([FromBody] DeviceRequest deviceRequest, Guid id)
	{
		var response = new BadRequestResult();
		return Task.FromResult((IActionResult)response);
	}

	[HttpDelete]
	[Route("devices/{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> DeleteDevice(Guid id)
	{
		var command = new DeleteDeviceCommand(id);
		await _mediator.HandleCommandAsync<DeleteDeviceCommand, DeleteDeviceResponse>(command);
		return NoContent();
	}
}
