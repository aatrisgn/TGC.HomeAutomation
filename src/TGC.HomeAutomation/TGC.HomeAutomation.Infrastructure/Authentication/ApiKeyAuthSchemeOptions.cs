using Microsoft.AspNetCore.Authentication;

namespace TGC.HomeAutomation.Infrastructure.Authentication;

public class ApiKeyAuthSchemeOptions : AuthenticationSchemeOptions
{
	public const string DefaultScheme = "ApiKeyAuthScheme";
	public string HeaderApiKey = "x-device-api-key";
	public string HeaderDeviceKey = "x-device-id";
}
