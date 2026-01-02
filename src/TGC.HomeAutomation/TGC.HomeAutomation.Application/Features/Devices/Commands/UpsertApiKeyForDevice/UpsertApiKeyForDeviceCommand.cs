using TGC.HomeAutomation.Application.Abstractions;

namespace TGC.HomeAutomation.Application.Features.Devices.Commands.UpsertApiKeyForDevice;

public class UpsertApiKeyForDeviceCommand : ICommand
{
	public Guid DeviceId { get; set; }
	public DateOnly ExpirationDate { get; set; }
	public required string Name { get; set; }
}