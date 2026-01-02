using TGC.HomeAutomation.Application.Features.Devices.Commands.UpsertApiKeyForDevice;

namespace TGC.HomeAutomation.Application.Abstractions;

public interface IApiKeyManager
{
	Task<bool> ValidateApiKeyAsync(Guid parsedDeviceId, string parsedSecret);
	Task<UpsertApiKeyForDeviceResponse> UpsertApiKeyAsync(UpsertApiKeyForDeviceCommand apiKeyRequest);
}