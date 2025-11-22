using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Caching.Memory;

namespace TGC.OpenWeatherApi;

internal class KeyvaultReaderService : IKeyvaultReaderService
{
	private readonly IOpenWeatherApiConfiguration _configuration;
	private readonly SecretClient _secretClient;
	
	private readonly MemoryCache _cache;
	
	public KeyvaultReaderService(IOpenWeatherApiConfiguration configuration)
	{
		_configuration = configuration;
		_secretClient = new SecretClient(new Uri(_configuration.KeyvaultUrl), new DefaultAzureCredential());
		
		_cache = new MemoryCache(new MemoryCacheOptions());
	}

	public async Task<string> GetOpenWeatherApiKeyAsync()
	{
		return await GetFromLocalCache(_configuration.KeyvaultSecretName);
	}

	private async Task<string> GetFromLocalCache(string key)
	{
		if (!_cache.TryGetValue(key, out string secretValue))
		{
			var cacheEntryOptions = new MemoryCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
				SlidingExpiration = null
			};
			
			KeyVaultSecret secret = await _secretClient.GetSecretAsync(_configuration.KeyvaultSecretName);
			
			_cache.Set(key, secret.Value, cacheEntryOptions);
			return secret.Value;
		}
		
		return secretValue;
	}
}