using TGC.HomeAutomation.API.Sensor;
using TGC.HomeAutomation.API.Temperature;
using TGC.WebApi.Communication;

namespace TGC.HomeAutomation.API.Device;

public interface IDeviceService
{
	Task<IEnumerable<DeviceResponse>> GetAllAsync();
	Task<ApiResult<DeviceResponse?>> GetByIdAsync(Guid id);
	Task<DeviceResponse> CreateAsync(DeviceRequest deviceRequest);
	Task<DeviceResponse> UpdateAsync(Guid id, DeviceRequest deviceRequest);
	Task<Guid> DeleteByIdAsync(Guid id);
	Task<DeviceEntity> GetByMacAddress(string requestMacAddress);
	Task<ApiKeyResponse> UpsertApiKeyAsync(ApiKeyRequest apiKeyRequest, Guid id);
	Task<DeviceMeasureTypesResponse> GetAvailableMeasureTypesByDeviceId(Guid id);
	Task<bool> ValidateApiKeyAsync(Guid parsedDeviceId, string parsedSecret);
	Task<ApiResult> CheckDeviceHealthAsync(Guid id);
}
