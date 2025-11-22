using TGC.AzureTableStorage;
using TGC.HomeAutomation.API.Measure;
using TGC.HomeAutomation.API.Sensor;
using TGC.HomeAutomation.API.Temperature;
using TGC.WebApi.Communication;

namespace TGC.HomeAutomation.API.Device;

public class DeviceService : IDeviceService
{
	private readonly IAzureTableStorageRepository<DeviceEntity> _deviceRepository;
	private readonly IAzureTableStorageRepository<ApiKeyEntity> _apiKeyRepository;
	private readonly IDeviceAPIKeyGenerator _deviceKeyGenerator;
	private readonly IDeviceCache _deviceCache;
	private readonly IOrderedMeasureService _orderedMeasureService;

	public DeviceService(
		IAzureTableStorageRepository<DeviceEntity> deviceRepository,
		IDeviceAPIKeyGenerator deviceKeyGenerator,
		IAzureTableStorageRepository<ApiKeyEntity> apiKeyRepository,
		IDeviceCache deviceCache,
		IOrderedMeasureService orderedMeasureService)
	{
		_deviceRepository = deviceRepository;
		_deviceKeyGenerator = deviceKeyGenerator;
		_apiKeyRepository = apiKeyRepository;
		_deviceCache = deviceCache;
		_orderedMeasureService = orderedMeasureService;
	}

	public async Task<IEnumerable<DeviceResponse>> GetAllAsync()
	{
		var allEntities = await _deviceRepository.GetAllAsync(e => true);
		return allEntities.Select(e => DeviceResponse.FromEntity(e)).ToList();
	}

	public async Task<ApiResult<DeviceResponse?>> GetByIdAsync(Guid id)
	{
		var allEntities = await _deviceRepository.GetAllAsync(e => true);
		var specificEntity = allEntities.SingleOrDefault(e => e.RowKey == id.ToString());
		var entityResponse = specificEntity == null ? null : DeviceResponse.FromEntity(specificEntity);
		return entityResponse is null ? ApiResult<DeviceResponse?>.AsNotFound() : ApiResult<DeviceResponse?>.AsOk(entityResponse);
	}

	public async Task<DeviceResponse> CreateAsync(DeviceRequest deviceRequest)
	{
		await _deviceRepository.CreateAsync(deviceRequest.ToEntity());
		var createdDevice = await _deviceRepository.GetSingleAsync(d => d.Name == deviceRequest.Name && d.MacAddress == deviceRequest.MacAddress);
		return DeviceResponse.FromEntity(createdDevice);
	}

	public Task<DeviceResponse> UpdateAsync(Guid id, DeviceRequest deviceRequest)
	{
		throw new NotImplementedException();
	}

	public async Task<Guid> DeleteByIdAsync(Guid id)
	{
		var allEntities = await _deviceRepository.GetAllAsync(e => true);
		var specificEntity = allEntities.Single(e => e.RowKey == id.ToString());
		return await _deviceRepository.DeleteAsync(specificEntity);
	}

	public async Task<DeviceEntity> GetByMacAddress(string requestMacAddress)
	{
		var entitiyMatch = await _deviceCache.GetEntity(requestMacAddress);
		return entitiyMatch;
	}

	public async Task<ApiKeyResponse> UpsertApiKeyAsync(ApiKeyRequest apiKeyRequest, Guid id)
	{
		var newApiKey = await _deviceKeyGenerator.GenerateDeviceAPIKey();
		var maskedApiKey = await _deviceKeyGenerator.MaskApiKey(newApiKey);

		var anyExistingApiKeys = await _apiKeyRepository.ExistsAsync(a => a.Name == apiKeyRequest.Name && a.DeviceId == id);

		if (anyExistingApiKeys)
		{
			var existingApiKey = await _apiKeyRepository.GetSingleAsync(d => d.Name == apiKeyRequest.Name && d.DeviceId == id);
			await _apiKeyRepository.DeleteByIdAsync(Guid.Parse(existingApiKey.RowKey!));
		}

		var newApiKeyEntity = new ApiKeyEntity
		{
			Secret = maskedApiKey.Secret,
			Salt = maskedApiKey.Salt,
			ExpirationDate = apiKeyRequest.ExpirationDate.ToDateTime(TimeOnly.MinValue).ToUniversalTime(),
			Name = apiKeyRequest.Name,
			DeviceId = id
		};

		await _apiKeyRepository.CreateAsync(newApiKeyEntity);

		return new ApiKeyResponse
		{
			Name = apiKeyRequest.Name,
			Secret = newApiKey,
			ExpirationDate = apiKeyRequest.ExpirationDate
		};
	}

	public async Task<DeviceMeasureTypesResponse> GetAvailableMeasureTypesByDeviceId(Guid id)
	{
		var allMeasures = await _orderedMeasureService.GetUniqueMeasureTypesByDeviceId(id);
		return new DeviceMeasureTypesResponse { DeviceId = id, MeasureTypes = allMeasures };
	}

	public async Task<bool> ValidateApiKeyAsync(Guid parsedDeviceId, string parsedSecret)
	{
		var device = await GetByIdAsync(parsedDeviceId);
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

	public Task<ApiResult> CheckDeviceHealthAsync(Guid id)
	{
		return Task.FromResult(ApiResult.AsNotFound());
	}
}
