using TGC.HomeAutomation.Application.Features.Devices.Commands.UpsertApiKeyForDevice;

namespace TGC.HomeAutomation.API.Contracts.Device;

public class ApiKeyRequest
{
	public DateOnly ExpirationDate { get; set; }
	public required string Name { get; set; }

	public UpsertApiKeyForDeviceCommand ToCommand(Guid id)
	{
		return new UpsertApiKeyForDeviceCommand
		{
			DeviceId = id,
			ExpirationDate = ExpirationDate,
			Name = Name
		};
	}
}
