using TGC.HomeAutomation.Application.Abstractions;

namespace TGC.HomeAutomation.Application.Features.Devices.Commands.DeleteDevice;

public class DeleteDeviceCommand : ICommand
{
	public Guid DeviceId { get; }

	public DeleteDeviceCommand(Guid deviceId)
	{
		DeviceId = deviceId;
	}
}
