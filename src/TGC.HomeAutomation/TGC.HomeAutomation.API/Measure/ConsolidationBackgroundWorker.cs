using TGC.AzureTableStorage;

namespace TGC.HomeAutomation.API.Measure;

public class ConsolidationBackgroundWorker : BackgroundService
{
	private readonly IAzureTableStorageRepository<MeasureEntity> _measureRepository;

	public ConsolidationBackgroundWorker(IAzureTableStorageRepository<MeasureEntity> measureRepository)
	{
		_measureRepository = measureRepository;
	}

	protected override Task ExecuteAsync(CancellationToken stoppingToken)
	{
		throw new NotImplementedException();
	}
}
