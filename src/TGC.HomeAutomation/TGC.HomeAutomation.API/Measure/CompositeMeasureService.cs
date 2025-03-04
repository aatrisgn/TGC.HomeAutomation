using TGC.AzureTableStorage;
using TGC.HomeAutomation.API.Device;

namespace TGC.HomeAutomation.API.Measure;

internal class CompositeMeasureService : ICompositeMeasureService
{
	private readonly TimeProvider _timeProvider;
	private readonly IMeasureTypeConverter _measureTypeConverter;

	private readonly IAzureTableStorageRepository<MeasureEntity> _measureRepository;
	private readonly IDeviceService _deviceService;

	public CompositeMeasureService(
		TimeProvider timeProvider,
		IMeasureTypeConverter measureTypeConverter,
		IAzureTableStorageRepository<MeasureEntity> measureRepository,
		IDeviceService deviceService
		)
	{
		_timeProvider = timeProvider;
		_measureRepository = measureRepository;
		_measureTypeConverter = measureTypeConverter;
		_deviceService = deviceService;
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
				Created = last30Minutes.DateTime,
				MacAddress = "N/A"
			};
		}

		return new MeasureResponse
		{
			Created = last30Minutes.DateTime,
			DataValue = 0,
			MacAddress = "N/A"
		};
	}

	public Task<MeasureResponse> GetCurrentOutside(string measureType)
	{
		var measure = new MeasureResponse
		{
			DataValue = 10,
			Created = _timeProvider.GetUtcNow().DateTime,
			DeviceId = Guid.Empty,
		};

		return Task.FromResult(measure);
	}

	public async Task<MeasureRangeResponse> GetAverageBy10Minutes(string measureType, DateTime startDate, DateTime endDate)
	{
		var temperatures = await _measureRepository.GetAllAsync(t => t.Created >= startDate && t.Created <= endDate);

		if (temperatures.Any())
		{
			var averages = temperatures
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
				MacAddress = "N/A"
			});

			return new MeasureRangeResponse { DataValues = measureDataValues, };
		}

		return new MeasureRangeResponse();
	}

	public async Task AddRead(MeasureRequest request)
	{
		var entityMeasure = await _measureTypeConverter.RequestToEntity(request, Guid.NewGuid());
		await _measureRepository.CreateAsync(entityMeasure);
	}
}
