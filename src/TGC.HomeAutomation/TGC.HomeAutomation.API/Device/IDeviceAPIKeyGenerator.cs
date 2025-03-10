namespace TGC.HomeAutomation.API.Device;

public interface IDeviceAPIKeyGenerator
{
	Task<string> GenerateDeviceAPIKey();
	Task<string> MaskApiKey(string apiKey);
}
