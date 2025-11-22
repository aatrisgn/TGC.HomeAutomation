namespace TGC.OpenWeatherApi;

public interface IOpenWeatherApiConfiguration
{
	string ApiKey { get; }
	bool UseKeyvault { get; }
	string KeyvaultUrl { get; }
	string KeyvaultSecretName { get; }
	bool Mock { get; }
}