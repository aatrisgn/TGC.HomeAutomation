using TGC.AzureTableStorage;

namespace TGC.HomeAutomation.API.Measure;

public class ConsolidationBackgroundWorker : BackgroundService
{
	private readonly ILogger<ConsolidationBackgroundWorker> _logger;
	private readonly IServiceProvider _serviceProvider;
	private readonly TimeProvider _timeProvider;

	private Timer? _timer = null;

	public ConsolidationBackgroundWorker(IServiceProvider services, ILogger<ConsolidationBackgroundWorker> logger, TimeProvider timeProvider)
	{
		_serviceProvider = services;
		_logger = logger;
		_timeProvider = timeProvider;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("Timed Hosted Service running.");

		// When the timer should have no due-time, then do the work once now.
		await ConsolidateMeasures();

		using PeriodicTimer timer = new(TimeSpan.FromSeconds(60));

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

			var timestampAnHourAgo = _timeProvider.GetUtcNow().AddHours(-1);

			var allMeasures = await rawMeasureRepository.GetAllAsync(m => m.Created < timestampAnHourAgo.DateTime);
			var orderedRawMeasures = allMeasures.OrderBy(m => m.Created).ToList();

			foreach (var rawMeasure in orderedRawMeasures)
			{
				var roundedDownDateTime = RoundDown(rawMeasure.Created, TimeSpan.FromMinutes(30));
				var roundedUpDateTime = roundedDownDateTime.AddMinutes(30);
				var measuresWithinTimeframe = orderedRawMeasures.Where(m => m.Created >= roundedDownDateTime && m.Created < roundedUpDateTime).GroupBy(m => m.DeviceId).ToList();
				
				_logger.LogInformation("Timed Hosted Service is working.");
			}
		}
		_logger.LogInformation("Timed Hosted Service is working.");
	}

	private DateTime RoundDown(DateTime dt, TimeSpan d)
	{
		return new DateTime((dt.Ticks / d.Ticks) * d.Ticks);
	}
}
