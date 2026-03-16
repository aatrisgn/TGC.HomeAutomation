using TGC.AzureTableStorage;
using TGC.HomeAutomation.API.Sensor;
using TGC.HomeAutomation.Application.Abstractions;
using TGC.WebApi.Communication.Mediator;

namespace TGC.HomeAutomation.Application.Features.Devices.Commands.CreateDevice;

public class CreateDeviceHandler : BaseCommandHandler<CreateDeviceCommand>, ICommandHandler
{
	private readonly IAzureTableStorageRepository<DeviceEntity> _deviceRepository;

	public CreateDeviceHandler(IAzureTableStorageRepository<DeviceEntity> deviceRepository)
	{
		_deviceRepository = deviceRepository;
	}

	public async Task<ICommandResponse> Handle<TCommand>(TCommand command) where TCommand : ICommand
	{
		var castedCommand = GetTypedCommand(command);

		var entity = new DeviceEntity
		{
			Name = castedCommand.Name,
			MacAddress = castedCommand.MacAddress,
			Created = DateTime.UtcNow
		};

		await _deviceRepository.CreateAsync(entity);
		
		var createdDevice = await _deviceRepository.GetSingleAsync(d => d.Name == castedCommand.Name && d.MacAddress == castedCommand.MacAddress);
		
		return CreateDeviceResponse.FromEntity(createdDevice);
	}
}
