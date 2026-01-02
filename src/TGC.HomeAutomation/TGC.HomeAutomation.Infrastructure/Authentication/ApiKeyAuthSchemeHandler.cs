using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TGC.AzureTableStorage;
using TGC.HomeAutomation.API.Sensor;
using TGC.HomeAutomation.Application.Abstractions;

namespace TGC.HomeAutomation.Infrastructure.Authentication;

public class ApiKeyAuthSchemeHandler : AuthenticationHandler<ApiKeyAuthSchemeOptions>
{
	private readonly IAzureTableStorageRepository<DeviceEntity> _deviceRepository;
	private readonly IDeviceLookup _deviceLookup;
	private readonly IApiKeyManager _apiKeyManager;

	public ApiKeyAuthSchemeHandler(
		IOptionsMonitor<ApiKeyAuthSchemeOptions> options,
		ILoggerFactory logger,
		UrlEncoder encoder,
		IAzureTableStorageRepository<DeviceEntity> deviceRepository,
		IApiKeyManager apiKeyManager,
		IDeviceLookup deviceLookup
		) : base(options, logger, encoder)
	{
		_deviceRepository = deviceRepository;
		_deviceLookup = deviceLookup;
		_apiKeyManager = apiKeyManager;
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

		var parsedDeviceId = Guid.Parse(deviceId.ToString());
		var parsedSecret = locatedAPIKey.ToString();

		var isValid = await _apiKeyManager.ValidateApiKeyAsync(parsedDeviceId, parsedSecret);

		if (!isValid)
		{
			return AuthenticateResult.Fail("Uauthorized");
		}

		var deviceResult = await _deviceLookup.GetByIdAsync(parsedDeviceId);
		var device = deviceResult;

		if (device is null)
		{
			return AuthenticateResult.Fail("Uauthorized");
		}

		//This should be changed to be based ApiKey info
		var claims = new[]
		{
			new Claim(ClaimTypes.Name, device.Name ?? string.Empty),
			new Claim(ClaimTypes.Upn, device.MacAddress ?? string.Empty),
			new Claim(ClaimTypes.Sid, device.RowKey),
		};
		var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "ApiKey"));
		var ticket = new AuthenticationTicket(principal, this.Scheme.Name);

		return AuthenticateResult.Success(ticket);
	}
}
