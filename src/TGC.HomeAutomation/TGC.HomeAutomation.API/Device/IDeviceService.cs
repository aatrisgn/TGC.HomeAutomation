using TGC.HomeAutomation.API.Sensor;
using TGC.WebApi.Communication;

namespace TGC.HomeAutomation.API.Device;

public interface IDeviceService
{
	Task<DeviceEntity> GetByMacAddress(string requestMacAddress);
	Task<ApiResult> CheckDeviceHealthAsync(Guid id);
}
