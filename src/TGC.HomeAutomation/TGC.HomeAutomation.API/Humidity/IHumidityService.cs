using TGC.HomeAutomation.API.Temperature;

namespace TGC.HomeAutomation.API.Humidity;

public interface IHumidityService
{
	Task<HumidityResponse> GetCurrentInside();
	Task<HumidityResponse> GetCurrentOutside();
	Task<IEnumerable<HumidityResponse>> GetAverageBy10Minutes(DateTime startDate, DateTime endDate);
	Task AddRead(HumidityRequest request);
}
