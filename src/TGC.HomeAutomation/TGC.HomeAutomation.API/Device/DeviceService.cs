using TGC.AzureTableStorage;
using TGC.HomeAutomation.API.Measure;
using TGC.HomeAutomation.API.Sensor;
using TGC.HomeAutomation.Application.Abstractions;
using TGC.WebApi.Communication;

namespace TGC.HomeAutomation.API.Device;

public class DeviceService : IDeviceService
{
	private readonly IAzureTableStorageRepository<DeviceEntity> _deviceRepository;
	private readonly IDeviceCache _deviceCache;
	private readonly IOrderedMeasureService _orderedMeasureService;

	public DeviceService(
		IAzureTableStorageRepository<DeviceEntity> deviceRepository,
		IDeviceCache deviceCache,
		IOrderedMeasureService orderedMeasureService)
	{
		_deviceRepository = deviceRepository;
		_deviceCache = deviceCache;
		_orderedMeasureService = orderedMeasureService;
	}

	public async Task<DeviceEntity> GetByMacAddress(string requestMacAddress)
	{
		var entitiyMatch = await _deviceCache.GetEntity(requestMacAddress);
		return entitiyMatch;
	}

	public Task<ApiResult> CheckDeviceHealthAsync(Guid id)
	{
		return Task.FromResult(ApiResult.AsNotFound());
	}
}
