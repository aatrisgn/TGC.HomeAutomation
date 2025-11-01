using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace TGC.HomeAutomation.Api.IntegrationTests.Tests;

public class NoJWTAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
	public const string SchemeName = JwtBearerDefaults.AuthenticationScheme;

	public NoJWTAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
		ILoggerFactory logger, UrlEncoder encoder)
		: base(options, logger, encoder)
	{
	}

	protected override Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		var claims = new[] { new Claim(ClaimTypes.Name, "Test user") };
		var identity = new ClaimsIdentity(claims, "Test");
		var principal = new ClaimsPrincipal(identity);
		var ticket = new AuthenticationTicket(principal, "TestScheme");

		var result = AuthenticateResult.Success(ticket);

		return Task.FromResult(result);
	}

	public static IServiceCollection Register(IServiceCollection services)
	{
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddScheme<AuthenticationSchemeOptions, NoJWTAuthenticationHandler>(
				JwtBearerDefaults.AuthenticationScheme, options =>
				{
				});
		return services;
	}
}
