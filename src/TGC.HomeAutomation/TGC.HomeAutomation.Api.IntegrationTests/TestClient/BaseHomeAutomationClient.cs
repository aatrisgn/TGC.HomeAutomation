using System.Net.Http.Headers;

namespace TGC.HomeAutomation.Api.IntegrationTests.TestClient;

public partial class BaseHomeAutomationClient
{
	protected Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken)
	{
		var message = new HttpRequestMessage();

		message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

		if (cancellationToken.IsCancellationRequested)
		{
			cancellationToken.ThrowIfCancellationRequested();
		}

		return Task.FromResult(message);
	}
}
