using TGC.AzureTableStorage;
using TGC.HomeAutomation.API.Sensor;
using TGC.HomeAutomation.Application.Abstractions;
using TGC.HomeAutomation.Application.Features.Devices.Commands.CreateDevice;
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
		var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

		var deviceExists = await deviceRepository.ExistsAsync(d => d.MacAddress == DeviceConstants.OutsideDeviceMacAddress && d.Name == DeviceConstants.OutsideDeviceName);

		if (!deviceExists)
		{
			_logger.LogInformation("SYSTEM device does not exist. New device will be created...");

			var newDevice = new CreateDeviceCommand(DeviceConstants.OutsideDeviceName, DeviceConstants.OutsideDeviceMacAddress);
			var result = await mediator.HandleCommandAsync<CreateDeviceCommand, CreateDeviceResponse>(newDevice);

			_logger.LogInformation("Created system device {name}:{macAddress}:{id}", result.Name, result.MacAddress, result.Id);
		}
		else
		{
			_logger.LogInformation("SYSTEM device already exist. No new device will be created.");
		}
	}
}
