using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using TGC.AzureTableStorage.IoC;
using TGC.HomeAutomation.API.Measure;

namespace TGC.HomeAutomation.Api.IntegrationTests;

public class HomeAutomationApiFixture : WebApplicationFactory<Program>, IAsyncLifetime
{
	public new async Task DisposeAsync()
	{
		await base.DisposeAsync();
	}

	public Task InitializeAsync()
	{
		return Task.CompletedTask;
	}

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.UseEnvironment("IntegrationTests");

		builder.ConfigureServices(services =>
		{
			var backgroundWorker = services.SingleOrDefault(
				d => d.ServiceType ==
					 typeof(ConsolidationBackgroundWorker));

			if (backgroundWorker is not null)
			{
				services.Remove(backgroundWorker);
			}

			services.AddAzureTableStorage(configuration =>
			{
				configuration.StubServices = true;
				configuration.AccountConnectionString = string.Empty;
			});
		});
	}
}
