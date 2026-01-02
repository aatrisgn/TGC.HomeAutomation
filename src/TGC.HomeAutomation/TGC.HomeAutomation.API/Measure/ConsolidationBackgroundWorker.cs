using Microsoft.Extensions.Options;
using TGC.OpenWeatherApi;
using TGC.AzureTableStorage;
using TGC.HomeAutomation.API.Configuration;
using TGC.HomeAutomation.API.Sensor;
using TGC.HomeAutomation.Domain.Constants;

namespace TGC.HomeAutomation.API.Measure;

public class ConsolidationBackgroundWorker : BackgroundService
{
	private readonly ILogger<ConsolidationBackgroundWorker> _logger;
	private readonly IServiceProvider _serviceProvider;
	private readonly TimeProvider _timeProvider;

	public ConsolidationBackgroundWorker(IServiceProvider services, ILogger<ConsolidationBackgroundWorker> logger, TimeProvider timeProvider)
	{
		_serviceProvider = services;
		_logger = logger;
		_timeProvider = timeProvider;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("Consolidating measures...");

		// When the timer should have no due-time, then do the work once now.
		await ConsolidateMeasures();

		using PeriodicTimer timer = new(TimeSpan.FromSeconds(600));

		try
		{
			while (await timer.WaitForNextTickAsync(stoppingToken))
			{
				await ConsolidateMeasures();
			}
		}
		catch (OperationCanceledException)
		{
			_logger.LogInformation("Timed Hosted Service is stopping.");
		}
	}

	private async Task ConsolidateMeasures()
	{
		using (var scope = _serviceProvider.CreateScope())
		{
			//Change implementation to get "raw measures" and make them "ordered" by 10 minute averages
			var rawMeasureRepository = scope.ServiceProvider.GetRequiredService<IAzureTableStorageRepository<MeasureEntity>>();
			var orderedMeasureRepository = scope.ServiceProvider.GetRequiredService<IAzureTableStorageRepository<OrderedMeasureEntity>>();

			var timestampAnHourAgo = _timeProvider.GetUtcNow().AddMinutes(-30);
			var roundedTimeAgo = RoundDown(timestampAnHourAgo.DateTime, TimeSpan.FromMinutes(30));

			// Locate all measures which are older than 1 hour to give a buffer
			var allMeasures = await rawMeasureRepository.GetAllAsync(m => m.Created < roundedTimeAgo);
			var orderedRawMeasures = allMeasures.OrderBy(m => m.Created).ToList();
			var lookUpMeasures = allMeasures.ToList();

			_logger.LogInformation("Located {rawMeasures} raw measures", orderedRawMeasures.Count);

			// Start by iterating over each and locate related measure within the same 30-minute period.
			// Then group by device ID to accumulate on individual device level.

			Task outsideTemperatureFetchingTask = FetchOutsideTemperatureAsync(scope, roundedTimeAgo.AddMinutes(30));

			foreach (var rawMeasure in orderedRawMeasures)
			{
				var roundedDownDateTime = RoundDown(rawMeasure.Created, TimeSpan.FromMinutes(30));
				var roundedUpDateTime = roundedDownDateTime.AddMinutes(30);

				var measuresWithinTimeframe = lookUpMeasures
					.Where(m => m.Created >= roundedDownDateTime && m.Created < roundedUpDateTime)
					.GroupBy(m => m.DeviceId)
					.ToList();

				_logger.LogInformation("Found {measuresWithinTimeframe} measures within {timespanStart} to {timespanEnd} timespan", measuresWithinTimeframe.Count, roundedDownDateTime, roundedUpDateTime);
				// Should be re-written
				foreach (var measure in measuresWithinTimeframe)
				{
					var measureSplittedByType = measure.GroupBy(m => m.Type).ToList();

					// Accumulate on each measure type since a single device can report on multiple measures (e.g. temperature and humidity)
					foreach (var measureType in measureSplittedByType)
					{
						var averageDataValue = measureType.Average(m => m.DataValue);

						var orderedMeasure = new OrderedMeasureEntity
						{
							DataValue = averageDataValue,
							Created = roundedDownDateTime,
							DeviceId = measure.Key,
							Type = measureType.Key ?? "N/A",
							Sample = measureType.Count()
						};

						//Create a single instance
						await orderedMeasureRepository.CreateAsync(orderedMeasure);
					}

					foreach (var measureToRemove in measure)
					{
						lookUpMeasures.Remove(measureToRemove);
					}
				}
			}

			foreach (var measure in allMeasures)
			{
				await rawMeasureRepository.DeleteAsync(measure);
			}

			await outsideTemperatureFetchingTask;
		}
		_logger.LogInformation("Finished processing measures.");
	}

	private async Task FetchOutsideTemperatureAsync(IServiceScope scope, DateTime time)
	{
		var deviceRepository = scope.ServiceProvider.GetRequiredService<IAzureTableStorageRepository<DeviceEntity>>();

		var openWeatherApiClient = scope.ServiceProvider.GetRequiredService<IOpenWeatherApiClient>();
		var configurationOptions = scope.ServiceProvider.GetRequiredService<IOptions<HomeAutomationConfiguration>>().Value;
		var orderedMeasureRepository = scope.ServiceProvider.GetRequiredService<IAzureTableStorageRepository<OrderedMeasureEntity>>();

		DeviceEntity? systemDevice = null;

		var deviceExists = await deviceRepository.ExistsAsync(d => d.MacAddress == DeviceConstants.OutsideDeviceMacAddress && d.Name == DeviceConstants.OutsideDeviceName);
		if (deviceExists)
		{
			systemDevice = await deviceRepository.GetSingleAsync(d => d.MacAddress == DeviceConstants.OutsideDeviceMacAddress);
		}

		if (systemDevice is not null)
		{
			var currentWeather = await openWeatherApiClient.GetCurrentWeatherAsync(configurationOptions.Coordinates.Latitude, configurationOptions.Coordinates.Longitude);

			var existingTemperatureMeasure = await orderedMeasureRepository.ExistsAsync(m => m.Created == time && m.Type == "temperature" && m.DeviceId == Guid.Parse(systemDevice.RowKey));
			var existingHumidityMeasure = await orderedMeasureRepository.ExistsAsync(m => m.Created == time && m.Type == "humidity" && m.DeviceId == Guid.Parse(systemDevice.RowKey));

			var result = currentWeather.Result();

			if (!existingTemperatureMeasure)
			{
				var outsideTemperature = new OrderedMeasureEntity
				{
					DataValue = result.WeatherDetails.Temp,
					Created = time,
					DeviceId = Guid.Parse(systemDevice.RowKey),
					Type = "temperature",
					Sample = 1
				};
				await orderedMeasureRepository.CreateAsync(outsideTemperature);
			}

			if (!existingHumidityMeasure)
			{
				var outsideHumidity = new OrderedMeasureEntity
				{
					DataValue = result.WeatherDetails.Humidity,
					Created = time,
					DeviceId = Guid.Parse(systemDevice.RowKey),
					Type = "humidity",
					Sample = 1
				};

				await orderedMeasureRepository.CreateAsync(outsideHumidity);
			}
		}
	}

	private DateTime RoundDown(DateTime dt, TimeSpan d)
	{
		return new DateTime((dt.Ticks / d.Ticks) * d.Ticks, DateTimeKind.Utc);
	}
}
