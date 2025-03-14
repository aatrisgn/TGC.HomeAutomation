namespace TGC.HomeAutomation.API.Measure;

public interface IOrderedMeasureService
{
	Task<DeviceOrderedMeasureRangeResponse> GetByDeviceId(Guid deviceId, DateTime startDate, DateTime endDate);
}
