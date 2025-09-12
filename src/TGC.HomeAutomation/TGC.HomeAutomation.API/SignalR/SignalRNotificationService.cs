using System.Text.Json;
using Microsoft.AspNetCore.SignalR;

namespace TGC.HomeAutomation.API.SignalR;

internal class SignalRNotificationService : ISignalRNotificationService
{
	private readonly IHubContext<AllClientsHub> _hubContext;

	public SignalRNotificationService(IHubContext<AllClientsHub> hubContext)
	{
		_hubContext = hubContext;
	}

	public async Task BroadcastMessageAsync<T>(T payload) where T : class
	{
		var serializedPayload = JsonSerializer.Serialize(payload, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
		await _hubContext.Clients.All.SendAsync("MeasureMessages", serializedPayload);
	}
}
