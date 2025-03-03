using TGC.AzureTableStorage;
using TGC.HomeAutomation.API.Temperature;

namespace TGC.HomeAutomation.API.Humidity;

public class HumidityService : IHumidityService
{
	private readonly IAzureTableStorageRepository<HumidityEntity> _humidityRepository;
	private readonly TimeProvider _timeProvider;

	public HumidityService(TimeProvider timeProvider, IAzureTableStorageRepository<HumidityEntity> humidityRepository)
	{
		_humidityRepository = humidityRepository;
		_timeProvider = timeProvider;
	}

	public async Task<HumidityResponse> GetCurrentInside()
	{
		var last30Minutes = _timeProvider.GetUtcNow().AddMinutes(-30);
		var currentHumidities = await _humidityRepository.GetAllAsync(t => t.Timestamp >= last30Minutes);

		if (currentHumidities.Any())
		{
			var averageHumidity = currentHumidities.Average(t => t.Humidity);

			return new HumidityResponse
			{
				Humidity = averageHumidity,
				Created = last30Minutes.DateTime,
				MacAddress = "N/A"
			};
		}

		return new HumidityResponse
		{
			Created = last30Minutes.DateTime,
			Humidity = 0,
			MacAddress = "N/A"
		};
	}

	public Task<HumidityResponse> GetCurrentOutside()
	{
		var humidity = new HumidityResponse
		{
			Humidity = 10,
			Created = _timeProvider.GetUtcNow().DateTime,
			MacAddress = "N/A"
		};

		return Task.FromResult(humidity);
	}

	public async Task<IEnumerable<HumidityResponse>> GetAverageBy10Minutes(DateTime startDate, DateTime endDate)
	{
		var humidity = await _humidityRepository.GetAllAsync(t => t.Created >= startDate && t.Created <= endDate);

		if (humidity.Any())
		{
			var averages = humidity
				.GroupBy(m => new { m.Created.Date, Hour = m.Created.Hour, TenMinuteInterval = m.Created.Minute / 10 })
				.Select(g => new
				{
					IntervalStart = g.Key.Date.AddHours(g.Key.Hour).AddMinutes(g.Key.TenMinuteInterval * 10),
					AverageMeasure = g.Average(m => m.Humidity)
				})
				.OrderBy(g => g.IntervalStart)
				.ToList();

			return averages.Select(a => new HumidityResponse
			{
				Humidity = a.AverageMeasure,
				Created = a.IntervalStart,
				MacAddress = "N/A"
			});
		}

		return [];
	}

	public Task AddRead(HumidityRequest request)
	{
		throw new NotImplementedException();
	}
}
