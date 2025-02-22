using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using TGC.AzureTableStorage;
using TGC.HomeAutomation.API.Sensor;

namespace TGC.HomeAutomation.API.Authentication;

public class ApiKeyAuthSchemeHandler : AuthenticationHandler<ApiKeyAuthSchemeOptions>
{
	private readonly IAPIKeyRepository _apiKeyRepository;
	private readonly IAzureTableStorageRepository<DeviceEntity> _deviceRepository;

	public ApiKeyAuthSchemeHandler(
		IOptionsMonitor<ApiKeyAuthSchemeOptions> options,
		ILoggerFactory logger,
		UrlEncoder encoder,
		IAPIKeyRepository apiKeyRepository,
		IAzureTableStorageRepository<DeviceEntity> deviceRepository
		) : base(options, logger, encoder)
	{
		_apiKeyRepository = apiKeyRepository;
		_deviceRepository = deviceRepository;
	}

	protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		if (Request.Headers.TryGetValue(Options.HeaderDeviceKey, out var deviceId) == false)
		{
			return AuthenticateResult.Fail("Uauthorized");
		}

		if (Request.Headers.TryGetValue(Options.HeaderApiKey, out var locatedAPIKey) == false)
		{
			return AuthenticateResult.Fail("Uauthorized");
		}

		var locatedApiKey = await _apiKeyRepository.GetByDeviceIdAsync(deviceId);

		if (locatedApiKey != locatedAPIKey)
		{
			return AuthenticateResult.Fail("Uauthorized");
		}

		var devices = await _deviceRepository.GetAllAsync(d => d.MacAddress == deviceId.ToString());

		var device = devices.SingleOrDefault();

		if (device is null)
		{
			return AuthenticateResult.Fail("Uauthorized");
		}

		//This should be changed to be based ApiKey info
		var claims = new[]
		{
			new Claim(ClaimTypes.Name, device.Name ?? string.Empty),
			new Claim(ClaimTypes.Upn, device.MacAddress ?? string.Empty),
			new Claim(ClaimTypes.Sid, device.RowKey ?? string.Empty),
		};
		var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "ApiKey"));
		var ticket = new AuthenticationTicket(principal, this.Scheme.Name);

		return AuthenticateResult.Success(ticket);
	}
}
