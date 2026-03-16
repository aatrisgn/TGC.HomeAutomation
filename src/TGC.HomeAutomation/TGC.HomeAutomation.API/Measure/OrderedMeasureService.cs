using TGC.AzureTableStorage;

namespace TGC.HomeAutomation.API.Measure;

public class OrderedMeasureService : IOrderedMeasureService
{
	private readonly IAzureTableStorageRepository<OrderedMeasureEntity> _orderedMeasureRepository;
	public OrderedMeasureService(IAzureTableStorageRepository<OrderedMeasureEntity> orderedMeasureRepository)
	{
		_orderedMeasureRepository = orderedMeasureRepository;
	}

	public async Task<DeviceOrderedMeasureRangeResponse> GetByDeviceId(Guid deviceId, DateTime startDate, DateTime endDate)
	{
		var locatedOrderedMeasures = await _orderedMeasureRepository
			.GetAllAsync(m =>
				m.DeviceId == deviceId
				&& m.Created > startDate
				&& m.Created < endDate);

		return new DeviceOrderedMeasureRangeResponse
		{
			DeviceId = deviceId,
			StartDate = startDate,
			EndDate = endDate,
			Measures = locatedOrderedMeasures.OrderBy(m => m.Created).Select(OrderedMeasureResponse.FromEntity)
		};
	}

	public async Task<DeviceOrderedMeasureRangeResponse> GetSpecificMeasuresByDeviceId(string measureType, Guid deviceId, DateTime startDate, DateTime endDate)
	{
		var locatedOrderedMeasures = await _orderedMeasureRepository
			.GetAllAsync(m =>
				m.DeviceId == deviceId
				&& m.Type == measureType
				&& m.Created > startDate
				&& m.Created < endDate);

		return new DeviceOrderedMeasureRangeResponse
		{
			DeviceId = deviceId,
			StartDate = startDate,
			EndDate = endDate,
			Measures = locatedOrderedMeasures.OrderBy(m => m.Created).Select(OrderedMeasureResponse.FromEntity)
		};
	}
}
