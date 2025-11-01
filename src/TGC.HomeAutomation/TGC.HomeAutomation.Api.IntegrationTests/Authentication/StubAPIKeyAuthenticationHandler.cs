using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TGC.HomeAutomation.API.Authentication;

namespace TGC.HomeAutomation.Api.IntegrationTests.Authentication;

public class StubAPIKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthSchemeOptions>
{
	public const string SchemeName = ApiKeyAuthSchemeOptions.DefaultScheme;

	public StubAPIKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
	{
	}

	protected override Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		throw new NotImplementedException();
	}
}
