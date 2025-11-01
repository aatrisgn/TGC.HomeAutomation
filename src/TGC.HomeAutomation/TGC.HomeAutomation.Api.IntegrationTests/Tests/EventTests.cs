using TGC.HomeAutomation.API.IntegrationTests.Generated;
using TGC.HomeAutomation.Api.IntegrationTests.TestClient;

namespace TGC.HomeAutomation.Api.IntegrationTests.Tests;

public class EventTests : BaseApiTest
{
	public EventTests(HomeAutomationApiFixture fixture) : base(fixture)
	{
	}

	[Fact]
	public async Task WHEN_GetAllEvents_GIVEN_NoToken_401IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.Build();

		var client = testContext.GetClientBuilder().Build();
		await client.Event_GetAllEventsAsync().AssertUnauthorized();
	}

	[Fact]
	public async Task WHEN_GetAllEvents_GIVEN_Token_200IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.ReplacePortalAuthWithStub()
			.Build();

		var client = testContext
			.GetClientBuilder()
			.WithJwtToken()
			.Build();

		await client.Event_GetAllEventsAsync().AssertOk();
	}

	[Fact]
	public async Task WHEN_GetAllEvents_GIVEN_BadToken_401IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.ReplacePortalAuthWithStub()
			.Build();

		var client = testContext
			.GetClientBuilder()
			.WithWrongJwtToken()
			.Build();

		await client.Event_GetAllEventsAsync().AssertUnauthorized();
	}

	[Fact]
	public async Task WHEN_GetAllEvents_GIVEN_ExpiredToken_401IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.ReplacePortalAuthWithStub()
			.Build();

		var client = testContext
			.GetClientBuilder()
			.WithExpiredJwtToken()
			.Build();

		await client.Event_GetAllEventsAsync().AssertUnauthorized();
	}

	[Fact]
	public async Task WHEN_GetAllEvents_GIVEN_ApiKey_401IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.ReplacePortalAuthWithStub()
			.Build();

		var client = testContext
			.GetClientBuilder()
			.WithJwtToken()
			.Build();

		var deviceRequest = new DeviceRequest
		{
			MacAddress = "123456",
			Name = "TestDevice"
		};

		var deviceReponse = await client.Device_CreateNewDeviceAsync(deviceRequest).AssertOk();

		var apiKeyResponse = await client.Device_UpdateApiKeyAsync(deviceReponse.Result.Id, new ApiKeyRequest
		{
			Name = "TestKey",
			ExpirationDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1))
		}).AssertOk();

		var apiKeyClient = testContext
			.ReuseClientBuilder()
			.WithAPIKeyToken(apiKeyResponse.Result.Secret, deviceReponse.Result.Id)
			.Build();

		await apiKeyClient.Event_GetAllEventsAsync().AssertUnauthorized();
	}
}
