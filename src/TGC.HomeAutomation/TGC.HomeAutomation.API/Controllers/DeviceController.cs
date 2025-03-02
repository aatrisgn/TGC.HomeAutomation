using Microsoft.AspNetCore.Mvc;
using TGC.HomeAutomation.API.Device;
using TGC.HomeAutomation.API.Sensor;
using TGC.HomeAutomation.API.Temperature;

namespace TGC.HomeAutomation.API.Controllers;

public class DeviceController : HAControllerBase
{
	private readonly IDeviceService _deviceService;
	public DeviceController(IDeviceService deviceService)
	{
		_deviceService = deviceService;
	}

	[HttpGet]
	[Route("devices")]
	[ProducesResponseType(typeof(IEnumerable<DeviceResponse>), StatusCodes.Status200OK)]
	public async Task<IEnumerable<DeviceResponse>> GetAllDevices()
	{
		return await _deviceService.GetAllAsync();
	}

	[HttpGet]
	[Route("devices/{id:guid}")]
	[ProducesResponseType(typeof(DeviceResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetSingleDeviceById(Guid id)
	{
		var locatedDevice = await _deviceService.GetByIdAsync(id);
		return locatedDevice != null ? Ok(locatedDevice) : NotFound();
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
	[Route("devices/{id:guid}")]
	[ProducesResponseType(typeof(DeviceResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<DeviceResponse> UpdateSingleDevice([FromBody] DeviceRequest deviceRequest, Guid id)
	{
		var updatedDevice = await _deviceService.UpdateAsync(id, deviceRequest);
		return updatedDevice;
	}

	[HttpDelete]
	[Route("devices/{id:guid}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task DeleteDevice(Guid id)
	{
		await _deviceService.DeleteByIdAsync(id);
	}
}
