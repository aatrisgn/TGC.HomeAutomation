namespace TGC.HomeAutomation.API.Measure;

public interface ICompositeMeasureService
{
	Task<MeasureResponse> GetCurrentMeasureInside(string measureType);
	Task<MeasureResponse> GetCurrentOutside(string measureType);
	Task<MeasureRangeResponse> GetAverageBy10Minutes(string measureType, DateTime startDate, DateTime endDate);
	Task AddRead(MeasureRequest request);
}
