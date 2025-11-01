using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace TGC.HomeAutomation.Api.IntegrationTests;

public class ApiServerTestContext<T> where T : class
{
	public readonly WebApplicationFactory<T> Fixture;
	private HttpClient? innerClient;

	public ApiServerTestContext(WebApplicationFactory<T> fixture)
	{
		Fixture = fixture;
	}

	public ApiServerTestContextBuilder<T> NewApiServerTextContextBuilder() =>
		new ApiServerTestContextBuilder<T>(Fixture);

	public TService GetService<TService>() where TService : class
		=> Fixture.Services.GetService<TService>() ?? throw new NullReferenceException($"Failed to resolve {typeof(TService)}");

	public IServiceScope CreateScope() => Fixture.Services.CreateScope();

	public HomeAutomationClientBuilder GetClientBuilder()
	{
		innerClient = Fixture.CreateClient();
		return new HomeAutomationClientBuilder(innerClient);
	}

	public HomeAutomationClientBuilder ReuseClientBuilder()
	{
		if (innerClient is null)
		{
			//Add better error description
			throw new CannotReuseTestHttpClientException();
		}

		return new HomeAutomationClientBuilder(innerClient);
	}
}
