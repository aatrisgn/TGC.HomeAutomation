using TGC.HomeAutomation.Application.Abstractions;
using TGC.WebApi.Communication.Mediator;

namespace TGC.HomeAutomation.Application.Features.Devices.Commands.UpsertApiKeyForDevice;

public class UpsertApiKeyForDeviceResponse : BaseResponse, ICommandResponse
{
	public DateOnly ExpirationDate { get; set; }
	public string? Name { get; set; }
	public string? Secret { get; set; }
}