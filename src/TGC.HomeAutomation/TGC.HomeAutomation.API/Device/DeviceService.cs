using TGC.AzureTableStorage;
using TGC.HomeAutomation.API.Sensor;
using TGC.HomeAutomation.API.Temperature;

namespace TGC.HomeAutomation.API.Device;

public class DeviceService : IDeviceService
{
	private readonly IAzureTableStorageRepository<DeviceEntity> _deviceRepository;

	public DeviceService(IAzureTableStorageRepository<DeviceEntity> deviceRepository)
	{
		_deviceRepository = deviceRepository;
	}

	public async Task<IEnumerable<DeviceResponse>> GetAllAsync()
	{
		var allEntities = await _deviceRepository.GetAllAsync(e => true);
		return allEntities.Select(e => DeviceResponse.FromEntity(e)).ToList();
	}

	public async Task<DeviceResponse?> GetByIdAsync(Guid id)
	{
		var allEntities = await _deviceRepository.GetAllAsync(e => true);
		var specificEntity = allEntities.SingleOrDefault(e => e.RowKey == id.ToString());
		return specificEntity == null ? null : DeviceResponse.FromEntity(specificEntity);
	}

	public Task<DeviceResponse> CreateAsync(DeviceRequest deviceRequest)
	{
		throw new NotImplementedException();
	}

	public Task<DeviceResponse> UpdateAsync(DeviceRequest deviceRequest)
	{
		throw new NotImplementedException();
	}

	public Task DeleteByIdAsync(Guid id)
	{
		throw new NotImplementedException();
	}
}
