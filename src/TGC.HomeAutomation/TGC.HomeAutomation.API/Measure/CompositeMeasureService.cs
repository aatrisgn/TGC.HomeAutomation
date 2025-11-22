using TGC.OpenWeatherApi;
using TGC.AzureTableStorage;
using TGC.HomeAutomation.API.Device;
using TGC.HomeAutomation.API.Sensor;
using TGC.HomeAutomation.API.SignalR;
using TGC.WebApi.Communication;

namespace TGC.HomeAutomation.API.Measure;

internal class CompositeMeasureService : ICompositeMeasureService
{
	private readonly TimeProvider _timeProvider;
	private readonly IMeasureTypeConverter _measureTypeConverter;
	private readonly IOrderedMeasureService _orderedMeasureService;
	private readonly IAzureTableStorageRepository<OrderedMeasureEntity> _orderedMeasureRepository;
	private readonly ISignalRNotificationService _notificationService;
	private readonly IOpenWeatherApiClient _openWeatherApiClient;

	private readonly IAzureTableStorageRepository<MeasureEntity> _measureRepository;
	private readonly IDeviceService _deviceService;

	public CompositeMeasureService(
		TimeProvider timeProvider,
		IMeasureTypeConverter measureTypeConverter,
		IAzureTableStorageRepository<MeasureEntity> measureRepository,
		IAzureTableStorageRepository<OrderedMeasureEntity> orderedMeasureRepository,
		IDeviceService deviceService,
		IOrderedMeasureService orderedMeasureService,
		ISignalRNotificationService notificationService,
		IOpenWeatherApiClient openWeatherApiClient
		)
	{
		_timeProvider = timeProvider;
		_measureRepository = measureRepository;
		_orderedMeasureRepository = orderedMeasureRepository;
		_measureTypeConverter = measureTypeConverter;
		_deviceService = deviceService;
		_orderedMeasureService = orderedMeasureService;
		_notificationService = notificationService;
		_openWeatherApiClient = openWeatherApiClient;
	}
	public async Task<MeasureResponse> GetCurrentMeasureInside(string measureType)
	{
		var convertedMeasureType = _measureTypeConverter.GetMeasureType(measureType);

		var last30Minutes = _timeProvider.GetUtcNow().AddMinutes(-30);
		var currentTemperatures = await _measureRepository.GetAllAsync(t => t.Created >= last30Minutes && t.Type == measureType);

		if (currentTemperatures.Any())
		{
			var averageTemperature = currentTemperatures.Average(t => t.DataValue);

			return new MeasureResponse
			{
				DataValue = Math.Round(averageTemperature, 1),
				Created = last30Minutes.DateTime
			};
		}

		return new MeasureResponse
		{
			Created = last30Minutes.DateTime,
			DataValue = 0
		};
	}

	public async Task<ApiResult> GetCurrentOutside(string measureType)
	{
		var apiResult = await _openWeatherApiClient.GetCurrentWeatherAsync("55.520099", "11.142788");

		var responseContent = apiResult.Result();

		if (measureType.Equals("temperature", StringComparison.CurrentCultureIgnoreCase))
		{
			var measure = new MeasureResponse
			{
				DataValue = responseContent.WeatherDetails.Temp,
				Created = _timeProvider.GetUtcNow().DateTime,
				DeviceId = Guid.Empty,
			};

			return ApiResult<MeasureResponse>.AsOk(measure);
		}
		if (measureType.Equals("humidity", StringComparison.CurrentCultureIgnoreCase))
		{
			var measure = new MeasureResponse
			{
				DataValue = responseContent.WeatherDetails.Humidity,
				Created = _timeProvider.GetUtcNow().DateTime,
				DeviceId = Guid.Empty,
			};

			return ApiResult<MeasureResponse>.AsOk(measure);
		}

		return ApiResult.AsNotFound();
	}

	public async Task<MeasureRangeResponse> GetAverageBy10Minutes(string measureType, DateTime startDate, DateTime endDate)
	{
		var outdoorDevice = await _deviceService.GetByMacAddress(DeviceConstants.OutsideDeviceMacAddress);

		var measures = await _orderedMeasureRepository.GetAllAsync(t => t.Created >= startDate && t.Created <= endDate && t.Type == measureType && t.DeviceId != Guid.Parse(outdoorDevice.RowKey));

		if (measures.Any())
		{
			var averages = measures
				.GroupBy(m => new { m.Created.Date, Hour = m.Created.Hour, TenMinuteInterval = m.Created.Minute / 10 })
				.Select(g => new
				{
					IntervalStart = g.Key.Date.AddHours(g.Key.Hour).AddMinutes(g.Key.TenMinuteInterval * 10),
					AverageMeasure = g.Average(m => m.DataValue)
				})
				.OrderBy(g => g.IntervalStart)
				.ToList();

			var measureDataValues = averages.Select(a => new MeasureResponse
			{
				DataValue = a.AverageMeasure,
				Created = a.IntervalStart,
				Type = measureType
			});

			return new MeasureRangeResponse { DataValues = measureDataValues, };
		}

		return new MeasureRangeResponse();
	}

	public async Task AddRead(MeasureRequest request)
	{
		await _notificationService.BroadcastMessageAsync(request);
		ArgumentNullException.ThrowIfNull(request.MacAddress);

		DeviceEntity originDevice = await _deviceService.GetByMacAddress(request.MacAddress);

		ArgumentNullException.ThrowIfNull(originDevice.RowKey);

		var entityMeasure = await _measureTypeConverter.RequestToEntity(request, Guid.Parse(originDevice.RowKey));
		await _measureRepository.CreateAsync(entityMeasure);
	}

	public async Task<DeviceOrderedMeasureRangeResponse> GetByDeviceId(Guid deviceId, DateTime startDate, DateTime endDate)
	{
		var deviceMeasures = await _orderedMeasureService.GetByDeviceId(deviceId, startDate, endDate);
		return deviceMeasures;
	}

	public async Task<MeasureRangeResponse> GetLatestActivityByDeviceId(Guid deviceId)
	{
		var last30Minutes = _timeProvider.GetUtcNow().AddMinutes(-30);
		var utcNow = _timeProvider.GetUtcNow().UtcDateTime;

		var deviceMeasures = await _measureRepository
			.GetAllAsync(m =>
				m.DeviceId == deviceId
				&& m.Created > last30Minutes
				&& m.Created < utcNow);

		return new MeasureRangeResponse
		{
			DataValues = deviceMeasures.Select(d =>
				{
					return new MeasureResponse
					{
						DataValue = d.DataValue,
						DeviceId = d.DeviceId,
						Created = d.Created,
						Type = d.Type
					};
				})
		};
	}

	public async Task<DeviceOrderedMeasureRangeResponse> GetSpecificMeasuresByDeviceIdForPeriod(string measureType, Guid deviceId, DateTime startDate, DateTime endDate)
	{
		var deviceMeasures = await _orderedMeasureService.GetSpecificMeasuresByDeviceId(measureType, deviceId, startDate, endDate);
		return deviceMeasures;
	}
}
