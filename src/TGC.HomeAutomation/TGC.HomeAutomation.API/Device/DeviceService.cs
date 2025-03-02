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

	public async Task<DeviceResponse> CreateAsync(DeviceRequest deviceRequest)
	{
		await _deviceRepository.CreateAsync(deviceRequest.ToEntity());
		var createdDevice = await _deviceRepository.GetSingleAsync(d => d.Name == deviceRequest.Name && d.MacAddress == deviceRequest.MacAddress);
		return DeviceResponse.FromEntity(createdDevice);
	}

	public Task<DeviceResponse> UpdateAsync(Guid id, DeviceRequest deviceRequest)
	{
		throw new NotImplementedException();
	}

	public Task DeleteByIdAsync(Guid id)
	{
		throw new NotImplementedException();
	}
}
