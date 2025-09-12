using TGC.HomeAutomation.API.Sensor;
using TGC.HomeAutomation.API.Temperature;

namespace TGC.HomeAutomation.API.Device;

public interface IDeviceService
{
	Task<IEnumerable<DeviceResponse>> GetAllAsync();
	Task<DeviceResponse?> GetByIdAsync(Guid id);
	Task<DeviceResponse> CreateAsync(DeviceRequest deviceRequest);
	Task<DeviceResponse> UpdateAsync(Guid id, DeviceRequest deviceRequest);
	Task<Guid> DeleteByIdAsync(Guid id);
	Task<DeviceEntity> GetByMacAddress(string requestMacAddress);
	Task<ApiKeyResponse> UpsertApiKeyAsync(ApiKeyRequest apiKeyRequest, Guid id);
	Task<DeviceMeasureTypesResponse> GetAvailableMeasureTypesByDeviceId(Guid id);
	Task<bool> ValidateApiKeyAsync(Guid parsedDeviceId, string parsedSecret);
}
