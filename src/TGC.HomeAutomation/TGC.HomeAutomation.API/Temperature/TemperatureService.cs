using TGC.AzureTableStorage;

namespace TGC.HomeAutomation.API.Temperature;

public class TemperatureService : ITemperatureService
{
	private readonly IAzureTableStorageRepository<TemperatureEntity> _temperatureRepository;
	private readonly TimeProvider _timeProvider;

	public TemperatureService(IAzureTableStorageRepository<TemperatureEntity> temperatureRepository, TimeProvider timeProvider)
	{
		_temperatureRepository = temperatureRepository;
		_timeProvider = timeProvider;
	}
	public async Task<TemperatureResponse> GetCurrentInside()
	{
		var last30Minutes = _timeProvider.GetUtcNow().AddMinutes(-30);
		var currentTemperatures = await _temperatureRepository.GetAllAsync(t => t.Timestamp >= last30Minutes);

		if (currentTemperatures.Any())
		{
			var averageTemperature = currentTemperatures.Average(t => t.Temperature);

			return new TemperatureResponse
			{
				Temperature = Math.Round(averageTemperature, 1),
				Created = last30Minutes.DateTime,
				MacAddress = "N/A"
			};
		}

		return new TemperatureResponse
		{
			Created = last30Minutes.DateTime,
			Temperature = 0,
			MacAddress = "N/A"
		};
	}

	public Task<TemperatureResponse> GetCurrentOutside()
	{
		var temperature = new TemperatureResponse
		{
			Temperature = 10,
			Created = _timeProvider.GetUtcNow().DateTime,
			MacAddress = "N/A"
		};

		return Task.FromResult(temperature);
	}

	public async Task<IEnumerable<TemperatureResponse>> GetAccumulatedByHour(DateTime startDate, DateTime endDate)
	{
		var temperatures = await _temperatureRepository.GetAllAsync(t => t.Created >= startDate && t.Created <= endDate);

		if (temperatures.Any())
		{
			var averages = temperatures
				.GroupBy(m => new { m.Created.Date, Hour = m.Created.Hour, TenMinuteInterval = m.Created.Minute / 10 })
				.Select(g => new
				{
					IntervalStart = g.Key.Date.AddHours(g.Key.Hour).AddMinutes(g.Key.TenMinuteInterval * 10),
					AverageMeasure = g.Average(m => m.Temperature)
				})
				.OrderBy(g => g.IntervalStart)
				.ToList();

			return averages.Select(a => new TemperatureResponse
			{
				Temperature = a.AverageMeasure,
				Created = a.IntervalStart,
				MacAddress = "N/A"
			});
		}

		return [];
	}

	public async Task AddRead(TemperatureRequest request)
	{
		await _temperatureRepository.CreateAsync(request.ToEntity());
	}
}
