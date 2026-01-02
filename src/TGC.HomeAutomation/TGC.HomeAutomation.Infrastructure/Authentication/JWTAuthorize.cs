using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace TGC.HomeAutomation.Infrastructure.Authentication;

public class JWTAuthorize : AuthorizeAttribute
{
	public JWTAuthorize()
	{
		AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
	}
}
