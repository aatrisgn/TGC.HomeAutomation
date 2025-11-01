using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TGC.HomeAutomation.Api.IntegrationTests.Authentication;
using TGC.HomeAutomation.API.IntegrationTests.Generated;

namespace TGC.HomeAutomation.Api.IntegrationTests;

public class HomeAutomationClientBuilder
{
	private readonly HttpClient _httpClient;
	private readonly List<Claim> _claims = new List<Claim>();

	public HomeAutomationClientBuilder(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public HomeAutomationClientBuilder WithWrongJwtToken()
	{
		GenerateJwtToken(StubTokenOptions.WrongTokenOptions());
		return this;
	}

	public HomeAutomationClientBuilder WithJwtToken()
	{
		GenerateJwtToken(StubTokenOptions.TokenOptions());
		return this;
	}

	public HomeAutomationClientBuilder WithExpiredJwtToken()
	{
		GenerateJwtToken(StubTokenOptions.ExpiredTokenOptions());
		return this;
	}

	private void ClearAuthenticationHeaders()
	{
		_httpClient.DefaultRequestHeaders.Remove("Authorization");
		_httpClient.DefaultRequestHeaders.Remove("x-device-api-key");
		_httpClient.DefaultRequestHeaders.Remove("x-device-id");
	}

	public HomeAutomationClientBuilder WithAPIKeyToken(string apiKey, Guid deviceId)
	{
		ClearAuthenticationHeaders();
		_httpClient.DefaultRequestHeaders.Add("x-device-api-key", apiKey);
		_httpClient.DefaultRequestHeaders.Add("x-device-id", deviceId.ToString());

		return this;
	}

	private void GenerateJwtToken(StubTokenOptions stubTokenOptions)
	{
		ClearAuthenticationHeaders();
		ArgumentNullException.ThrowIfNull(nameof(stubTokenOptions));

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(stubTokenOptions.SigningKey));
		var creds = new SigningCredentials(key, stubTokenOptions.Algorithm);

		var expirationDate = stubTokenOptions.Expired ? TimeSpan.FromMinutes(-5) : TimeSpan.FromMinutes(5);

		var jwtToken = new JwtSecurityToken(
			issuer: stubTokenOptions.Issuer,
			audience: stubTokenOptions.Audience,
			claims: _claims,
			expires: DateTime.UtcNow.Add(expirationDate),
			signingCredentials: creds
		);

		var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

		_httpClient.DefaultRequestHeaders.Authorization =
			new AuthenticationHeaderValue("Bearer", token);
	}

	public HomeAutomationApiClient Build()
	{
		return new HomeAutomationApiClient(_httpClient.BaseAddress!.AbsoluteUri, _httpClient);
	}
}
