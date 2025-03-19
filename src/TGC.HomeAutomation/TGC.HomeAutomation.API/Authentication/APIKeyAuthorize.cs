using Microsoft.AspNetCore.Authorization;

namespace TGC.HomeAutomation.API.Authentication;

public class APIKeyAuthorize : AuthorizeAttribute
{
	public APIKeyAuthorize()
	{
		AuthenticationSchemes = ApiKeyAuthSchemeOptions.DefaultScheme;
	}
}
