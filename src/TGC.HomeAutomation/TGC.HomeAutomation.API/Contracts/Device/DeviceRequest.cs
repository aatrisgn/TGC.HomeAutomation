using TGC.HomeAutomation.API.Sensor;
using TGC.HomeAutomation.Application.Features.Devices.Commands.CreateDevice;

namespace TGC.HomeAutomation.API.Contracts.Device;

public record DeviceRequest
{
	public string? Name { get; set; }
	public string? MacAddress { get; set; }

	public DeviceEntity ToEntity()
	{
		return new DeviceEntity { Name = Name, MacAddress = MacAddress, Created = DateTime.UtcNow };
	}

	public CreateDeviceCommand ToCommand()
	{
		return new CreateDeviceCommand(Name, MacAddress);
	}
}
