using TGC.HomeAutomation.API.IntegrationTests.Generated;
using TGC.HomeAutomation.Api.IntegrationTests.TestClient;

namespace TGC.HomeAutomation.Api.IntegrationTests.Tests;

public class MeasureTests : BaseApiTest
{
	public MeasureTests(HomeAutomationApiFixture fixture) : base(fixture)
	{
	}

	[Fact]
	public async Task WHEN_CreatingMeasure_GIVEN_NoTokenAndNoAuth_200IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.ReplacePortalAuthWithEmpty()
			.ReplaceApiKeyAuthWithEmpty()
			.Build();

		var client = testContext.GetClientBuilder().Build();

		var deviceRequest = new DeviceRequest
		{
			MacAddress = "123456",
			Name = "TestDevice"
		};

		await client.Device_CreateNewDeviceAsync(deviceRequest).AssertOk();

		var measureRequest = new MeasureRequest
		{
			Type = "Temperature",
			MacAddress = "123456",
			DataValue = 10
		};

		await client.Measure_CreateAsync(measureRequest).AssertOk();
	}

	[Fact]
	public async Task WHEN_CreatingMeasure_GIVEN_WrongToken_401IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.Build();

		var client = testContext
			.GetClientBuilder()
			.WithWrongJwtToken()
			.Build();

		await client.Measure_CreateAsync(new MeasureRequest()).AssertUnauthorized();
	}

	[Fact]
	public async Task WHEN_CreatingMeasure_GIVEN_ValidApiKey_200IsReturned()
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

		var apiKey = await client.Device_UpdateApiKeyAsync(deviceReponse.Result.Id, new ApiKeyRequest
		{
			Name = "TestKey",
			ExpirationDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1))
		}).AssertOk();

		var measureRequest = new MeasureRequest
		{
			Type = "Temperature",
			MacAddress = "123456",
			DataValue = 10
		};

		var apiKeyClient = testContext
			.ReuseClientBuilder()
			.WithAPIKeyToken(apiKey.Result.Secret, deviceReponse.Result.Id)
			.Build();

		await apiKeyClient.Measure_CreateAsync(measureRequest).AssertOk();
	}

	[Fact]
	public async Task WHEN_CreatingMeasure_GIVEN_ValidToken_401IsReturned()
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

		await client.Device_UpdateApiKeyAsync(deviceReponse.Result.Id, new ApiKeyRequest
		{
			Name = "TestKey",
			ExpirationDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1))
		}).AssertOk();

		var measureRequest = new MeasureRequest
		{
			Type = "Temperature",
			MacAddress = "123456",
			DataValue = 10
		};

		await client.Measure_CreateAsync(measureRequest).AssertUnauthorized();
	}

	[Fact]
	public async Task WHEN_CreatingMeasure_GIVEN_BadApiKey_401IsReturned()
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

		await client.Device_UpdateApiKeyAsync(deviceReponse.Result.Id, new ApiKeyRequest
		{
			Name = "TestKey",
			ExpirationDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1))
		}).AssertOk();

		var measureRequest = new MeasureRequest
		{
			Type = "Temperature",
			MacAddress = "123456",
			DataValue = 10
		};

		var apiKeyClient = testContext
			.ReuseClientBuilder()
			.WithAPIKeyToken("InvalidKey", deviceReponse.Result.Id)
			.Build();

		await apiKeyClient.Measure_CreateAsync(measureRequest).AssertUnauthorized();
	}

	[Fact]
	public async Task WHEN_CreatingMeasure_GIVEN_NoToken_401IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.Build();

		var client = testContext.GetClientBuilder().Build();
		await client.Measure_CreateAsync(new MeasureRequest()).AssertUnauthorized();
	}

	[Fact]
	public async Task WHEN_GetCurrentInsideMeasure_GIVEN_NoToken_401IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.Build();

		var client = testContext.GetClientBuilder().Build();
		await client.Measure_GetCurrentInsideAsync("temperature").AssertUnauthorized();
	}

	[Fact]
	public async Task WHEN_GetMeasureByDate_GIVEN_NoToken_401IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.Build();

		var client = testContext.GetClientBuilder().Build();
		await client.Measure_GetMeasuresByDateAsync("temperature", DateTime.Now, DateTime.Now).AssertUnauthorized();
	}

	[Fact]
	public async Task WHEN_GetCurrentOutside_GIVEN_NoToken_401IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.Build();

		var client = testContext.GetClientBuilder().Build();
		await client.Measure_GetCurrentOutsideAsync("temperature").AssertUnauthorized();
	}

	[Fact]
	public async Task WHEN_GetLatestActivityByDeviceId_GIVEN_NoToken_401IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.Build();

		var client = testContext.GetClientBuilder().Build();
		await client.Measure_GetLatestActivityByDeviceIdAsync(Guid.NewGuid()).AssertUnauthorized();
	}

	[Fact]
	public async Task WHEN_GetMeasuresByDeviceIdAndDate_GIVEN_NoToken_401IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.Build();

		var client = testContext.GetClientBuilder().Build();
		await client.Measure_GetMeasuresByDeviceIdAndDateAsync(Guid.NewGuid(), DateTime.Now, DateTime.Now).AssertUnauthorized();
	}

	[Fact]
	public async Task WHEN_GetMeasuresByDeviceIdMeasureTypeAndDate_GIVEN_NoToken_401IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.Build();

		var client = testContext.GetClientBuilder().Build();
		await client.Measure_GetMeasuresByDeviceIdMeasureTypeAndDateAsync(Guid.NewGuid(), "Temperature", DateTime.Now, DateTime.Now).AssertUnauthorized();
	}
}
