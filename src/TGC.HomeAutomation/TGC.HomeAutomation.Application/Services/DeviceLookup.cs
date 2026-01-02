using TGC.AzureTableStorage;
using TGC.HomeAutomation.API.Device;
using TGC.HomeAutomation.API.Sensor;
using TGC.HomeAutomation.Application.Abstractions;

namespace TGC.HomeAutomation.Application.Services;

public class DeviceLookup : IDeviceLookup
{
	private readonly IAzureTableStorageRepository<DeviceEntity> _deviceRepository;
	private readonly IDeviceCache _deviceCache;
	
	public DeviceLookup(IAzureTableStorageRepository<DeviceEntity> deviceRepository, IDeviceCache deviceCache)
	{
		_deviceCache = deviceCache;
		_deviceRepository = deviceRepository;
	}
	
	public async Task<IEnumerable<DeviceEntity>> GetAllAsync()
	{
		var allEntities = await _deviceRepository.GetAllAsync(e => true);
		return allEntities;
	}

	public async Task<DeviceEntity> GetByIdAsync(Guid id)
	{
		var allEntities = await _deviceRepository.GetAllAsync(e => true);
		var specificEntity = allEntities.SingleOrDefault(e => e.RowKey == id.ToString());
		return specificEntity;
	}
	
	public async Task<DeviceEntity> GetByMacAddress(string requestMacAddress)
	{
		var entitiyMatch = await _deviceCache.GetEntity(requestMacAddress);
		return entitiyMatch;
	}
}