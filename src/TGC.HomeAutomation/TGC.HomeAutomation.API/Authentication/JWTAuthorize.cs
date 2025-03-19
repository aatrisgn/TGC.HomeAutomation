using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace TGC.HomeAutomation.API.Authentication;

public class JWTAuthorize : AuthorizeAttribute
{
	public JWTAuthorize()
	{
		AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
	}
}
