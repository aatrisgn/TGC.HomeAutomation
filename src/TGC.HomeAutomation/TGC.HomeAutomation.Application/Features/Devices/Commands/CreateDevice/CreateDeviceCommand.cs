using TGC.HomeAutomation.Application.Abstractions;

namespace TGC.HomeAutomation.Application.Features.Devices.Commands.CreateDevice;

public class CreateDeviceCommand : ICommand
{
	public string? Name { get; }
	public string? MacAddress { get; }

	public CreateDeviceCommand(string? name, string? macAddress)
	{
		Name = name;
		MacAddress = macAddress;
	}
}
