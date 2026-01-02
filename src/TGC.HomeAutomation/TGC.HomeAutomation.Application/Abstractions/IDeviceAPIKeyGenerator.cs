using TGC.HomeAutomation.API.Device;

namespace TGC.HomeAutomation.Application.Abstractions;

public interface IDeviceApiKeyGenerator
{
	Task<string> GenerateDeviceAPIKey();
	Task<MaskedApiKey> MaskApiKey(string apiKey);
	Task<MaskedApiKey> MaskApiKey(string apiKey, byte[] salt);
}
