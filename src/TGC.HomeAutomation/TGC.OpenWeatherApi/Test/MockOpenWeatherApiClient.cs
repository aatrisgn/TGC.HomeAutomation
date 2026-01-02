using TGC.OpenWeatherApi.Models;
using TGC.WebApi.Communication;

namespace TGC.OpenWeatherApi.Test;

public class MockOpenWeatherApiClient : IOpenWeatherApiClient
{
	public Task<ApiResult<WeatherResponse?>> GetCurrentWeatherAsync(string latitude, string longitude)
	{
		var mockResponse = new WeatherResponse
		{
			WeatherDetails = new WeatherDetailsResponse
			{
				FeelsLike = 10
			}
		};
		return Task.FromResult(ApiResult<WeatherResponse?>.AsOk(mockResponse));
	}
}