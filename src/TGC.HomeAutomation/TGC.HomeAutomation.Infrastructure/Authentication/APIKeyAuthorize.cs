using Microsoft.AspNetCore.Authorization;

namespace TGC.HomeAutomation.Infrastructure.Authentication;

public class ApiKeyAuthorize : AuthorizeAttribute
{
	public ApiKeyAuthorize()
	{
		AuthenticationSchemes = ApiKeyAuthSchemeOptions.DefaultScheme;
	}
}
