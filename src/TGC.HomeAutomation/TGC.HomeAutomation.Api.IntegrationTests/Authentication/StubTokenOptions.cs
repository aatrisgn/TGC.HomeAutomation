using Microsoft.IdentityModel.Tokens;

namespace TGC.HomeAutomation.Api.IntegrationTests.Authentication;

public class StubTokenOptions
{
	public string Audience { get; init; } = string.Empty;
	public string Issuer { get; init; } = string.Empty;
	public string SigningKey { get; init; } = string.Empty;
	public string Algorithm => SecurityAlgorithms.HmacSha256;
	public bool Expired { get; init; }

	public static StubTokenOptions WrongTokenOptions()
	{
		return new StubTokenOptions
		{
			Audience = AuthenticationConstants.WrongAudience,
			Issuer = AuthenticationConstants.WrongIssuer,
			SigningKey = AuthenticationConstants.WrongSigningKey,
			Expired = false
		};
	}

	public static StubTokenOptions TokenOptions()
	{
		return new StubTokenOptions
		{
			Audience = AuthenticationConstants.Audience,
			Issuer = AuthenticationConstants.Issuer,
			SigningKey = AuthenticationConstants.SigningKey,
			Expired = false
		};
	}

	public static StubTokenOptions ExpiredTokenOptions()
	{
		return new StubTokenOptions
		{
			Audience = AuthenticationConstants.Audience,
			Issuer = AuthenticationConstants.Issuer,
			SigningKey = AuthenticationConstants.SigningKey,
			Expired = true
		};
	}
}
