namespace TGC.HomeAutomation.API.Temperature;

public interface ITemperatureService
{
	Task<TemperatureResponse> GetCurrentInside();
	Task<TemperatureResponse> GetCurrentOutside();
	Task<IEnumerable<TemperatureResponse>> GetAccumulatedByHour(DateTime startDate, DateTime endDate);
	Task AddRead(TemperatureRequest request);
}
