using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace TGC.HomeAutomation.Api.IntegrationTests.Authentication;

public class StubJWTAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
	public StubJWTAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
	{
	}

	protected override Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		if (Request.Headers.TryGetValue("Authorization", out var authorizationHeaderValue))
		{
			var jwtHeaderValue = authorizationHeaderValue.First();
			if (!jwtHeaderValue!.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
			{
				return Task.FromResult(AuthenticateResult.NoResult());
			}

			string token = jwtHeaderValue.Substring("Bearer ".Length).Trim();

			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidIssuer = AuthenticationConstants.Issuer,

				ValidateAudience = true,
				ValidAudience = AuthenticationConstants.Audience,

				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthenticationConstants.SigningKey)),

				ValidateLifetime = true,
				ClockSkew = TimeSpan.Zero
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			try
			{
				var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

				// Optionally check if the token is a JwtSecurityToken
				if (validatedToken is JwtSecurityToken jwtToken)
				{
					var issuer = jwtToken.Issuer;
					var audience = jwtToken.Audiences.FirstOrDefault();
					// You can also access claims from `principal`
				}

				return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name)));
			}
			catch (Exception ex)
			{
				return Task.FromResult(AuthenticateResult.Fail($"Token validation failed: {ex.Message}"));
			}
		}
		return Task.FromResult(AuthenticateResult.NoResult());
	}

	public static IServiceCollection Register(IServiceCollection services)
	{
		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddScheme<AuthenticationSchemeOptions, StubJWTAuthenticationHandler>(
				JwtBearerDefaults.AuthenticationScheme, options =>
				{
				});
		return services;
	}
}
