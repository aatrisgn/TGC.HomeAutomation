namespace TGC.HomeAutomation.API.Device;

public interface IDeviceAPIKeyGenerator
{
	Task<string> GenerateDeviceAPIKey();
	Task<MaskedApiKey> MaskApiKey(string apiKey);
	Task<MaskedApiKey> MaskApiKey(string apiKey, byte[] salt);
}
