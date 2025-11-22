using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using TGC.OpenWeatherApi.Models;
using TGC.WebApi.Communication;

namespace TGC.OpenWeatherApi;

internal class OpenWeatherApiClient : IOpenWeatherApiClient
{
	private readonly IKeyvaultReaderService _keyvaultReaderService;
	private readonly HttpClient _httpClient;
	private readonly MemoryCache _cache;
	
	private MemoryCacheEntryOptions memoryCacheEntryOptions => new MemoryCacheEntryOptions
	{
		AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
		SlidingExpiration = null
	};
	
	public OpenWeatherApiClient(IKeyvaultReaderService keyvaultReaderService, HttpClient httpClient)
	{
		_keyvaultReaderService = keyvaultReaderService;
		_httpClient = httpClient;
		
		_cache = new MemoryCache(new MemoryCacheOptions());
	}
	
	public async Task<ApiResult<WeatherResponse>> GetCurrentWeatherAsync(string latitude, string longitude)
	{
		var result = await GetFromLocalCache(latitude, longitude);
		return result is null ? ApiResult<WeatherResponse>.AsNotFound() : ApiResult<WeatherResponse>.AsOk(result);
	}
	
	private async Task<WeatherResponse?> GetFromLocalCache(string latitude, string longitude)
	{
		var apiKey = await _keyvaultReaderService.GetOpenWeatherApiKeyAsync();
		var cacheKey = $"{apiKey}_{latitude}_{longitude}";
		
		if (!_cache.TryGetValue(cacheKey, out string weatherApiPayloadJson))
		{
			var response = await _httpClient.GetAsync($"weather?lat={latitude}&lon={longitude}&appid={apiKey}&units=metric");
			if (!response.IsSuccessStatusCode)
			{
				return null;
			}
		
			var content = await response.Content.ReadAsStringAsync();
			_cache.Set(cacheKey, content, memoryCacheEntryOptions);

			return JsonSerializer.Deserialize<WeatherResponse>(content);
		}
		
		return JsonSerializer.Deserialize<WeatherResponse>(weatherApiPayloadJson);
	}
}