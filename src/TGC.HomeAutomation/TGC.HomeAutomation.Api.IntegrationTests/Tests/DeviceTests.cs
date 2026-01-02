using TGC.HomeAutomation.API.IntegrationTests.Generated;
using TGC.HomeAutomation.Api.IntegrationTests.TestClient;

namespace TGC.HomeAutomation.Api.IntegrationTests.Tests;

public class DeviceTests : BaseApiTest
{
	public DeviceTests(HomeAutomationApiFixture fixture) : base(fixture)
	{
	}

	[Fact]
	public async Task WHEN_GettingAllDevices_AllDevicesAreReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.ReplacePortalAuthWithStub()
			.Build();

		var client = testContext.GetClientBuilder().WithJwtToken().Build();

		await client.Device_GetAllDevicesAsync().AssertOk();
	}

	[Fact]
	public async Task WHEN_GettingSingleDeviceById_GIVEN_ValidToken_200IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.ReplacePortalAuthWithStub()
			.Build();

		var client = testContext.GetClientBuilder().WithJwtToken().Build();

		var deviceRequest = new DeviceRequest
		{
			MacAddress = "123456",
			Name = "TestDevice"
		};

		var deviceResponse = await client.Device_CreateNewDeviceAsync(deviceRequest).AssertOk();

		await client.Device_GetSingleDeviceByIdAsync(deviceResponse.Result.Id).AssertOk();
	}

	[Fact]
	public async Task WHEN_GettingHealthCheckById_GIVEN_ValidToken_404IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.ReplacePortalAuthWithStub()
			.Build();

		var client = testContext.GetClientBuilder().WithJwtToken().Build();

		var deviceRequest = new DeviceRequest
		{
			MacAddress = "123456",
			Name = "TestDevice"
		};

		var deviceResponse = await client.Device_CreateNewDeviceAsync(deviceRequest).AssertOk();

		// Currently returns 404 NotFound as per implementation
		await client.Device_GetHealthCheckByIdAsync(deviceResponse.Result.Id).AssertNotFound();
	}

	[Fact]
	public async Task WHEN_GettingAvailableMeasuresByDevice_GIVEN_ValidToken_200IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.ReplacePortalAuthWithStub()
			.Build();

		var client = testContext.GetClientBuilder().WithJwtToken().Build();

		var deviceRequest = new DeviceRequest
		{
			MacAddress = "123456",
			Name = "TestDevice"
		};

		var deviceResponse = await client.Device_CreateNewDeviceAsync(deviceRequest).AssertOk();

		await client.Device_GetAvailableMeasuresByDeviceIdAsync(deviceResponse.Result.Id).AssertOk();
	}

	[Fact]
	public async Task WHEN_CreatingNewDevice_GIVEN_ValidToken_200IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.ReplacePortalAuthWithStub()
			.Build();

		var client = testContext.GetClientBuilder().WithJwtToken().Build();

		var deviceRequest = new DeviceRequest
		{
			MacAddress = "123456",
			Name = "TestDevice"
		};

		await client.Device_CreateNewDeviceAsync(deviceRequest).AssertOk();
	}

	[Fact]
	public async Task WHEN_UpdatingDeviceApiKey_GIVEN_ValidToken_200IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.ReplacePortalAuthWithStub()
			.Build();

		var client = testContext.GetClientBuilder().WithJwtToken().Build();

		var deviceRequest = new DeviceRequest
		{
			MacAddress = "123456",
			Name = "TestDevice"
		};

		var deviceResponse = await client.Device_CreateNewDeviceAsync(deviceRequest).AssertOk();

		await client.Device_UpdateApiKeyAsync(deviceResponse.Result.Id, new ApiKeyRequest
		{
			Name = "TestKey",
			ExpirationDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1))
		}).AssertOk();
	}

	[Fact]
	public async Task WHEN_UpdatingSingleDevice_GIVEN_ValidToken_400IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.ReplacePortalAuthWithStub()
			.Build();

		var client = testContext.GetClientBuilder().WithJwtToken().Build();

		var deviceRequest = new DeviceRequest
		{
			MacAddress = "123456",
			Name = "TestDevice"
		};

		var deviceResponse = await client.Device_CreateNewDeviceAsync(deviceRequest).AssertOk();

		// Currently returns 400 BadRequest as per implementation
		await client.Device_UpdateSingleDeviceAsync(deviceResponse.Result.Id, deviceRequest).AssertBadRequest();
	}

	[Fact]
	public async Task WHEN_DeletingDevice_GIVEN_ValidToken_204IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.ReplacePortalAuthWithStub()
			.Build();

		var client = testContext.GetClientBuilder().WithJwtToken().Build();

		var deviceRequest = new DeviceRequest
		{
			MacAddress = "123456",
			Name = "TestDevice"
		};

		var deviceResponse = await client.Device_CreateNewDeviceAsync(deviceRequest).AssertOk();

		await client.Device_DeleteDeviceAsync(deviceResponse.Result.Id).AssertNoContent();
	}

	[Fact]
	public async Task WHEN_GettingAllDevices_GIVEN_NoToken_401IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.Build();

		var client = testContext.GetClientBuilder().Build();
		await client.Device_GetAllDevicesAsync().AssertUnauthorized();
	}

	[Fact]
	public async Task WHEN_DeletingDevice_GIVEN_NoToken_401IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.Build();

		var client = testContext.GetClientBuilder().Build();
		await client.Device_DeleteDeviceAsync(Guid.NewGuid()).AssertUnauthorized();
	}

	[Fact]
	public async Task WHEN_CreatingNewDevice_GIVEN_NoToken_401IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.Build();

		var client = testContext.GetClientBuilder().Build();
		await client.Device_CreateNewDeviceAsync(new DeviceRequest()).AssertUnauthorized();
	}

	[Fact]
	public async Task WHEN_UpdatingDeviceApiKey_GIVEN_NoToken_401IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.Build();

		var client = testContext.GetClientBuilder().Build();
		await client.Device_UpdateApiKeyAsync(Guid.NewGuid(), new ApiKeyRequest()).AssertUnauthorized();
	}

	[Fact]
	public async Task WHEN_UpdatingSingleDevice_GIVEN_NoToken_401IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.Build();

		var client = testContext.GetClientBuilder().Build();
		await client.Device_UpdateSingleDeviceAsync(Guid.NewGuid(), new DeviceRequest()).AssertUnauthorized();
	}

	[Fact]
	public async Task WHEN_GettingSingleDeviceById_GIVEN_NoToken_401IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.Build();

		var client = testContext.GetClientBuilder().Build();
		await client.Device_GetSingleDeviceByIdAsync(Guid.NewGuid()).AssertUnauthorized();
	}

	[Fact]
	public async Task WHEN_GettingAvailableMeasuresByDevice_GIVEN_NoToken_401IsReturned()
	{
		var testContext = _testContext
			.NewApiServerTextContextBuilder()
			.Build();

		var client = testContext.GetClientBuilder().Build();
		await client.Device_GetAvailableMeasuresByDeviceIdAsync(Guid.NewGuid()).AssertUnauthorized();
	}
}
