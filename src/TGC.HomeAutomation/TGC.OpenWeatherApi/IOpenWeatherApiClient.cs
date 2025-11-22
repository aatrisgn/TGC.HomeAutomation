using TGC.OpenWeatherApi.Models;
using TGC.WebApi.Communication;

namespace TGC.OpenWeatherApi;

public interface IOpenWeatherApiClient
{
	Task<ApiResult<WeatherResponse>> GetCurrentWeatherAsync(string latitude, string longitude);
}