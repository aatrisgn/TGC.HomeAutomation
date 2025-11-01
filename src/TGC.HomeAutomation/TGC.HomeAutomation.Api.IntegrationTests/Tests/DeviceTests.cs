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
			.ReplacePortalAuthWithEmpty()
			.Build();

		var client = testContext.GetClientBuilder().Build();

		await client.Device_GetAllDevicesAsync().AssertOk();
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
