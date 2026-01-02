using TGC.AzureTableStorage;
using TGC.HomeAutomation.API.Device;
using TGC.HomeAutomation.Application.Abstractions;
using TGC.HomeAutomation.Application.Features.Devices.Commands.UpsertApiKeyForDevice;

namespace TGC.HomeAutomation.Application.Services;

public class ApiKeyManager : IApiKeyManager
{
	private readonly IDeviceApiKeyGenerator _deviceKeyGenerator;
	private readonly IAzureTableStorageRepository<ApiKeyEntity> _apiKeyRepository;
	private readonly IDeviceLookup _deviceLookup; 

	public ApiKeyManager(IDeviceApiKeyGenerator deviceKeyGenerator, IDeviceLookup deviceLookup, IAzureTableStorageRepository<ApiKeyEntity> apiKeyRepository)
	{
		_deviceKeyGenerator = deviceKeyGenerator;
		_deviceLookup = deviceLookup;
		_apiKeyRepository = apiKeyRepository;
	}
	
	public async Task<bool> ValidateApiKeyAsync(Guid parsedDeviceId, string parsedSecret)
	{
		var device = await _deviceLookup.GetByIdAsync(parsedDeviceId);
		var apiKey = await _apiKeyRepository.GetSingleAsync(d => d.DeviceId == parsedDeviceId);
		var maskedKey = await _deviceKeyGenerator.MaskApiKey(parsedSecret, apiKey.Salt);

		var anyExistingApiKeys = await _apiKeyRepository.ExistsAsync(a => a.Secret == maskedKey.Secret && a.DeviceId == parsedDeviceId);
		//var apiKey = await _apiKeyRepository.GetSingleAsync(d => d.Secret == maskedKey && d.DeviceId == parsedDeviceId);

		if (device is null)
		{
			return false;
		}

		if (!anyExistingApiKeys)
		{
			return false;
		}

		if (apiKey is null)
		{
			return false;
		}

		if (!apiKey.Secret.Equals(maskedKey.Secret))
		{
			return false;
		}

		return true;
	}
	
	public async Task<UpsertApiKeyForDeviceResponse> UpsertApiKeyAsync(UpsertApiKeyForDeviceCommand apiKeyRequest)
	{
		var newApiKey = await _deviceKeyGenerator.GenerateDeviceAPIKey();
		var maskedApiKey = await _deviceKeyGenerator.MaskApiKey(newApiKey);

		var anyExistingApiKeys = await _apiKeyRepository.ExistsAsync(a => a.Name == apiKeyRequest.Name && a.DeviceId == apiKeyRequest.DeviceId);

		if (anyExistingApiKeys)
		{
			var existingApiKey = await _apiKeyRepository.GetSingleAsync(d => d.Name == apiKeyRequest.Name && d.DeviceId == apiKeyRequest.DeviceId);
			await _apiKeyRepository.DeleteByIdAsync(Guid.Parse(existingApiKey.RowKey!));
		}

		var newApiKeyEntity = new ApiKeyEntity
		{
			Secret = maskedApiKey.Secret,
			Salt = maskedApiKey.Salt,
			ExpirationDate = apiKeyRequest.ExpirationDate.ToDateTime(TimeOnly.MinValue).ToUniversalTime(),
			Name = apiKeyRequest.Name,
			DeviceId = apiKeyRequest.DeviceId
		};

		await _apiKeyRepository.CreateAsync(newApiKeyEntity);

		return new UpsertApiKeyForDeviceResponse
		{
			Name = apiKeyRequest.Name,
			Secret = newApiKey,
			ExpirationDate = apiKeyRequest.ExpirationDate
		};
	}
}