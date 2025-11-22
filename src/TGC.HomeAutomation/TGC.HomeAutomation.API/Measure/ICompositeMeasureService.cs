using TGC.WebApi.Communication;

namespace TGC.HomeAutomation.API.Measure;

public interface ICompositeMeasureService
{
	Task<MeasureResponse> GetCurrentMeasureInside(string measureType);
	Task<ApiResult> GetCurrentOutside(string measureType);
	Task<MeasureRangeResponse> GetAverageBy10Minutes(string measureType, DateTime startDate, DateTime endDate);
	Task AddRead(MeasureRequest request);
	Task<DeviceOrderedMeasureRangeResponse> GetByDeviceId(Guid deviceId, DateTime startDate, DateTime endDate);
	Task<MeasureRangeResponse> GetLatestActivityByDeviceId(Guid deviceId);
	Task<DeviceOrderedMeasureRangeResponse> GetSpecificMeasuresByDeviceIdForPeriod(string measureType, Guid deviceId, DateTime startDate, DateTime endDate);
}
