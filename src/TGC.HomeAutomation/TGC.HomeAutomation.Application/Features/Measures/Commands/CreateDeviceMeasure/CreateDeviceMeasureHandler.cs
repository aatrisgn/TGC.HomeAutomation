using TGC.AzureTableStorage;
using TGC.HomeAutomation.API.Measure;
using TGC.HomeAutomation.API.Sensor;
using TGC.HomeAutomation.Application.Abstractions;

namespace TGC.HomeAutomation.Application.Features.Measures.Commands.CreateDeviceMeasure;

public class CreateDeviceMeasureHandler : BaseCommandHandler<CreateDeviceMeasureCommand>, ICommandHandler
{
	public CreateDeviceMeasureCommand Command { get; set; }
	public CreateDeviceMeasureResponse CommandResponse { get; set; }
	
	private readonly IAzureTableStorageRepository<MeasureEntity> _measureRepository;
	private readonly ISignalRNotificationService _notificationService;
	private readonly IDeviceLookup _deviceLookupService;
	private readonly TimeProvider _timeProvider;

	public CreateDeviceMeasureHandler(
		IAzureTableStorageRepository<MeasureEntity> measureRepository,
		ISignalRNotificationService notificationService,
		IDeviceLookup deviceLookupService,
		TimeProvider timeProvider)
	{
		_measureRepository = measureRepository;
		_notificationService = notificationService;
		_deviceLookupService = deviceLookupService;
		_timeProvider = timeProvider;
	}
	
	public async Task<ICommandResponse> Handle<TCommand>(TCommand command) where TCommand : ICommand
	{
		var castedCommand = command as CreateDeviceMeasureCommand;
		
		await _notificationService.BroadcastMessageAsync(castedCommand);
		ArgumentNullException.ThrowIfNull(castedCommand.MacAddress);

		DeviceEntity originDevice = await _deviceLookupService.GetByMacAddress(castedCommand.MacAddress);

		ArgumentNullException.ThrowIfNull(originDevice.RowKey);

		var entityMeasure = castedCommand.ToMeasureEntity(Guid.Parse(originDevice.RowKey), _timeProvider.GetUtcNow().UtcDateTime);
		await _measureRepository.CreateAsync(entityMeasure);

		return new CreateDeviceMeasureResponse();
	}
}