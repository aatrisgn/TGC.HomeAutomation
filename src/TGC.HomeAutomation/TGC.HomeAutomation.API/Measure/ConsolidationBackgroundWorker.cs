using TGC.AzureTableStorage;
using TGC.HomeAutomation.API.Sensor;

namespace TGC.HomeAutomation.API.Measure;

public class ConsolidationBackgroundWorker : BackgroundService
{
	private readonly ILogger<ConsolidationBackgroundWorker> _logger;
	private readonly IServiceProvider _serviceProvider;

	private Timer? _timer = null;

	public ConsolidationBackgroundWorker(IServiceProvider services, ILogger<ConsolidationBackgroundWorker> logger)
	{
		_serviceProvider = services;
		_logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("Timed Hosted Service running.");

		// When the timer should have no due-time, then do the work once now.
		await ConsolidateMeasures();

		using PeriodicTimer timer = new(TimeSpan.FromSeconds(20));

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
			var deviceRepository = scope.ServiceProvider.GetRequiredService<IAzureTableStorageRepository<DeviceEntity>>();

			await deviceRepository.GetAllAsync(d => d.IsActive);
		}
		_logger.LogInformation("Timed Hosted Service is working.");
	}
}
