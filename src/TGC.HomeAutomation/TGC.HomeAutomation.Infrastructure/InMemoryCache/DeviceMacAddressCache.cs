using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using TGC.AzureTableStorage;
using TGC.HomeAutomation.API.Device;
using TGC.HomeAutomation.API.Sensor;
using TGC.HomeAutomation.Application.Abstractions;

namespace TGC.HomeAutomation.Infrastructure.InMemoryCache;

public class DeviceMacAddressCache : IDeviceCache
{
	private readonly IMemoryCache _memoryCache;
	private readonly ILogger<DeviceMacAddressCache> _logger;
	private readonly IAzureTableStorageRepository<DeviceEntity> _deviceRepository;

	private MemoryCacheEntryOptions _options;

	public DeviceMacAddressCache(IMemoryCache memoryCache, IAzureTableStorageRepository<DeviceEntity> deviceRepository, ILogger<DeviceMacAddressCache> logger)
	{
		_memoryCache = memoryCache;
		_deviceRepository = deviceRepository;
		_logger = logger;

		_options = new MemoryCacheEntryOptions()
			.SetSlidingExpiration(TimeSpan.FromSeconds(120));
	}

	public async Task<DeviceEntity> GetEntity(string key)
	{
		if (!_memoryCache.TryGetValue(key, out DeviceEntity? device) || device is null)
		{
			device = await _deviceRepository.GetSingleAsync(d => d.MacAddress == key);
			_memoryCache.Set(key, device, _options);

			_logger.LogTrace($"Device {key} has been cached.");

			return device;
		}

		_logger.LogTrace($"Device returned from Cache: {key}");
		return device;
	}
}
