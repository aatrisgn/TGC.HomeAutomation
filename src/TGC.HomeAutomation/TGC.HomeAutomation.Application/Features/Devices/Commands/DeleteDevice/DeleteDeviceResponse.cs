using TGC.HomeAutomation.Application.Abstractions;

namespace TGC.HomeAutomation.Application.Features.Devices.Commands.DeleteDevice;

public class DeleteDeviceResponse : ICommandResponse
{
	public Guid DeviceId { get; }

	public DeleteDeviceResponse(Guid deviceId)
	{
		DeviceId = deviceId;
	}
}
