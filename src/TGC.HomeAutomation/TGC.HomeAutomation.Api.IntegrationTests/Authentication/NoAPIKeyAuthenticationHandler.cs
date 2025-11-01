using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TGC.HomeAutomation.API.Authentication;

namespace TGC.HomeAutomation.Api.IntegrationTests.Authentication;

public class NoAPIKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthSchemeOptions>
{
	public const string SchemeName = ApiKeyAuthSchemeOptions.DefaultScheme;

	public NoAPIKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
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
		services.AddAuthentication(SchemeName)
			.AddScheme<ApiKeyAuthSchemeOptions, NoAPIKeyAuthenticationHandler>(
				SchemeName, options =>
				{
				});
		return services;
	}
}
