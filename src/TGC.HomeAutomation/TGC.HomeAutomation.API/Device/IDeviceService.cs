using TGC.HomeAutomation.API.Contracts.Device;
using TGC.HomeAutomation.API.Sensor;
using TGC.WebApi.Communication;

namespace TGC.HomeAutomation.API.Device;

public interface IDeviceService
{
	Task<DeviceEntity> GetByMacAddress(string requestMacAddress);
	Task<DeviceResponse> CreateAsync(DeviceRequest deviceRequest);
	Task<DeviceMeasureTypesResponse> GetAvailableMeasureTypesByDeviceId(Guid id);
	Task<ApiResult> CheckDeviceHealthAsync(Guid id);
}
