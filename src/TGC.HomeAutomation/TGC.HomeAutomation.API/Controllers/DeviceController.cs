using Microsoft.AspNetCore.Mvc;

namespace TGC.HomeAutomation.API.Controllers;

public class DeviceController : HAControllerBase
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public Task<string> GetAllDevices()
	{
		return Task.FromResult("OK");
	}

	[HttpGet]
	[Route("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public Task<string> GetSingleDeviceById(Guid id)
	{
		return Task.FromResult("OK");
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public Task<string> CreateNewDevice()
	{
		return Task.FromResult("OK");
	}

	[HttpPut]
	[Route("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public Task<string> UpdateSingleDevice(Guid id)
	{
		return Task.FromResult("OK");
	}

	[HttpDelete]
	[Route("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public Task<string> DeleteDevice(Guid id)
	{
		return Task.FromResult("OK");
	}
}
