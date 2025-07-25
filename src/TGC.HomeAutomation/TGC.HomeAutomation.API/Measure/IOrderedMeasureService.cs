namespace TGC.HomeAutomation.API.Measure;

public interface IOrderedMeasureService
{
	Task<DeviceOrderedMeasureRangeResponse> GetByDeviceId(Guid deviceId, DateTime startDate, DateTime endDate);
	Task<IEnumerable<string>> GetUniqueMeasureTypesByDeviceId(Guid id);
	Task<DeviceOrderedMeasureRangeResponse> GetSpecificMeasuresByDeviceId(string measureType, Guid deviceId, DateTime startDate, DateTime endDate);
}
