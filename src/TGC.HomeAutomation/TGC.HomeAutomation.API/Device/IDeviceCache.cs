using TGC.HomeAutomation.API.Sensor;

namespace TGC.HomeAutomation.API.Device;

//TODO: This could be considered making more generic if more usage of caching is needed.
public interface IDeviceCache
{
	Task<DeviceEntity> GetEntity(string key);
}
