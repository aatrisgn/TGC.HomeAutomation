using TGC.HomeAutomation.Application.Abstractions;
using TGC.WebApi.Communication.Mediator;

namespace TGC.HomeAutomation.Application.Features.Devices.Commands.DeleteDevice;

public class DeleteDeviceResponse : BaseResponse, ICommandResponse
{
	public Guid DeviceId { get; }

	public DeleteDeviceResponse(Guid deviceId)
	{
		DeviceId = deviceId;
	}

	public DeleteDeviceResponse()
	{
		
	}
}
