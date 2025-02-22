using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TGC.HomeAutomation.API;

public class AuthorizationHeaderParameterOperationFilter : IOperationFilter
{
	public void Apply(OpenApiOperation operation, OperationFilterContext context)
	{
		operation.Security ??= new List<OpenApiSecurityRequirement>();

		var xDeviceApiKey = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "x-device-api-key" } };
		var userId = new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "x-device-id" } };
		operation.Security.Add(new OpenApiSecurityRequirement
		{
			[xDeviceApiKey] = new List<string>(),
			[userId] = new List<string>()
		});
	}
}
