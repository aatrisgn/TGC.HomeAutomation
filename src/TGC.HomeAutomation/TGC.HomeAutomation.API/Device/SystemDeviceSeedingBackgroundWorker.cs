using TGC.AzureTableStorage;
using TGC.HomeAutomation.API.Contracts.Device;
using TGC.HomeAutomation.API.Sensor;
using TGC.HomeAutomation.Domain.Constants;

namespace TGC.HomeAutomation.API.Device;

public class SystemDeviceSeedingBackgroundWorker : BackgroundService
{
	private readonly IServiceProvider _serviceProvider;
	private readonly ILogger<SystemDeviceSeedingBackgroundWorker> _logger;

	public SystemDeviceSeedingBackgroundWorker(IServiceProvider serviceProvider, ILogger<SystemDeviceSeedingBackgroundWorker> logger)
	{
		_serviceProvider = serviceProvider;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("Creating system devices...");

		using var scope = _serviceProvider.CreateScope();
		var deviceRepository = scope.ServiceProvider.GetRequiredService<IAzureTableStorageRepository<DeviceEntity>>();
		var deviceService = scope.ServiceProvider.GetRequiredService<IDeviceService>();

		var deviceExists = await deviceRepository.ExistsAsync(d => d.MacAddress == DeviceConstants.OutsideDeviceMacAddress && d.Name == DeviceConstants.OutsideDeviceName);

		if (!deviceExists)
		{
			_logger.LogInformation("SYSTEM device does not exist. New device will be created...");

			var newDevice = new DeviceRequest { MacAddress = DeviceConstants.OutsideDeviceMacAddress, Name = DeviceConstants.OutsideDeviceName, };
			var result = await deviceService.CreateAsync(newDevice);

			_logger.LogInformation("Created system device {name}:{macAddress}:{id}", result.Name, result.MacAddress, result.Id);
		}
		else
		{
			_logger.LogInformation("SYSTEM device already exist. No new device will be created.");
		}
	}
}
