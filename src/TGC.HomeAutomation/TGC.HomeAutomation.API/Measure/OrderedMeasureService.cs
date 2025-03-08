namespace TGC.HomeAutomation.API.Measure;

public class OrderedMeasureService : IOrderedMeasureService
{
	public Task<DeviceOrderedMeasureRangeResponse> GetByDeviceId(Guid deviceId, DateTime startDate, DateTime endDate)
	{
		throw new NotImplementedException();
	}
}
