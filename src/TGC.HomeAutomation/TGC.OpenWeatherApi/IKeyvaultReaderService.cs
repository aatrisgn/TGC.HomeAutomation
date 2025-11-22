namespace TGC.OpenWeatherApi;

public interface IKeyvaultReaderService
{
	public Task<string> GetOpenWeatherApiKeyAsync();
}