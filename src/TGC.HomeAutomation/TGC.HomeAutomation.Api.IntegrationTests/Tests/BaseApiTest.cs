namespace TGC.HomeAutomation.Api.IntegrationTests.Tests;

public abstract class BaseApiTest : IClassFixture<HomeAutomationApiFixture>, IAsyncLifetime
{
	protected readonly ApiServerTestContext<Program> _testContext;

	private readonly HomeAutomationApiFixture _fixture;

	public BaseApiTest(HomeAutomationApiFixture fixture)
	{
		_testContext = new ApiServerTestContext<Program>(fixture);
		_fixture = fixture;
	}

	public async Task InitializeAsync()
	{
		await PreconfigureTests();
	}

	public Task DisposeAsync()
	{
		return Task.CompletedTask;
	}

	protected virtual ValueTask PreconfigureTests() { return ValueTask.CompletedTask; }
}
