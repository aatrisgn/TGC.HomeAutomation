namespace TGC.HomeAutomation.API.Device;

public interface IDeviceAPIKeyGenerator
{
	Task<DeviceAPIKey> GenerateDeviceAPIKey();
}
