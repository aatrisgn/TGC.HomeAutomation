namespace TGC.OpenWeatherApi;

public class OpenWeatherApiConfiguration : IOpenWeatherApiConfiguration
{
	public string ApiKey { get; set; }
	public bool UseKeyvault { get; set; }
	public string KeyvaultUrl { get; set; }
	public string KeyvaultSecretName { get; set; }
	public bool Mock { get; set; }
	public bool UseManagedIdentity { get; set; }
	public Guid ManagedIdentityClientId { get; set; }
}