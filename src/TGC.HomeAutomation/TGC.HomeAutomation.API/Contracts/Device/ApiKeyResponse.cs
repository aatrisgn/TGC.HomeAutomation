using TGC.HomeAutomation.Application.Features.Devices.Commands.UpsertApiKeyForDevice;

namespace TGC.HomeAutomation.API.Contracts.Device;

public class ApiKeyResponse
{
	public DateOnly ExpirationDate { get; set; }
	public required string Name { get; set; }
	public required string Secret { get; set; }

	public static ApiKeyResponse FromCommand(UpsertApiKeyForDeviceResponse result)
	{
		return new ApiKeyResponse
		{
			ExpirationDate = result.ExpirationDate,
			Name = result.Name,
			Secret = result.Secret
		};
	}
}
