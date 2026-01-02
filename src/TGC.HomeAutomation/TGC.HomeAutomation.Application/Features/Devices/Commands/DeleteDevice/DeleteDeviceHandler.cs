using TGC.AzureTableStorage;
using TGC.HomeAutomation.API.Sensor;
using TGC.HomeAutomation.Application.Abstractions;

namespace TGC.HomeAutomation.Application.Features.Devices.Commands.DeleteDevice;

public class DeleteDeviceHandler : BaseCommandHandler<DeleteDeviceCommand>, ICommandHandler
{
	private readonly IAzureTableStorageRepository<DeviceEntity> _deviceRepository;

	public DeleteDeviceHandler(IAzureTableStorageRepository<DeviceEntity> deviceRepository)
	{
		_deviceRepository = deviceRepository;
	}

	public async Task<ICommandResponse> Handle<TCommand>(TCommand command) where TCommand : ICommand
	{
		var castedCommand = GetTypedCommand(command);
		
		ArgumentNullException.ThrowIfNull(castedCommand.DeviceId);
		
		var allEntities = await _deviceRepository.GetAllAsync(e => true);
		var specificEntity = allEntities.Single(e => e.RowKey == castedCommand.DeviceId.ToString());
		var deletedId = await _deviceRepository.DeleteAsync(specificEntity);
		return new DeleteDeviceResponse(deletedId);
	}
}
