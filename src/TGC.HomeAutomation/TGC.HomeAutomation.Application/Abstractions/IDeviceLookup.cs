using TGC.HomeAutomation.API.Sensor;

namespace TGC.HomeAutomation.Application.Abstractions;

public interface IDeviceLookup
{
	Task<IEnumerable<DeviceEntity>> GetAllAsync();
	Task<DeviceEntity> GetByIdAsync(Guid id);
	Task<DeviceEntity> GetByMacAddress(string requestMacAddress);
}