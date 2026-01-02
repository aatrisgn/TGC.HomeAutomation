using TGC.HomeAutomation.Application.Abstractions;

namespace TGC.HomeAutomation.Application.Features.Devices.Commands.UpsertApiKeyForDevice;

public class UpsertApiKeyForDeviceResponse : ICommandResponse
{
	public DateOnly ExpirationDate { get; set; }
	public required string Name { get; set; }
	public required string Secret { get; set; }
}