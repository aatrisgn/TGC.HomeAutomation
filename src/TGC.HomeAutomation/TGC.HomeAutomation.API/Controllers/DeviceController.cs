using Microsoft.AspNetCore.Mvc;

namespace TGC.HomeAutomation.API.Controllers;

[ApiController]
[Route("[controller]")]
public class DeviceController : ControllerBase
{
	[HttpGet]
	public Task<string> GetAllDevices()
	{
		return Task.FromResult("OK");
	}

	[HttpGet]
	[Route("{id}")]
	public Task<string> GetSingleDeviceById(Guid id)
	{
		return Task.FromResult("OK");
	}

	[HttpPost]
	public Task<string> CreateNewDevice()
	{
		return Task.FromResult("OK");
	}

	[HttpPut]
	[Route("{id}")]
	public Task<string> UpdateSingleDevice(Guid id)
	{
		return Task.FromResult("OK");
	}

	[HttpDelete]
	[Route("{id}")]
	public Task<string> DeleteDevice(Guid id)
	{
		return Task.FromResult("OK");
	}
}
