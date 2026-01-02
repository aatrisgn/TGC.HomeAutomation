using Microsoft.AspNetCore.SignalR;
using TGC.HomeAutomation.Infrastructure.Authentication;

namespace TGC.HomeAutomation.Infrastructure.SignalR;

[JWTAuthorize]
internal class AllClientsHub : Hub
{
	//Not exposed but kept as example for now.
	// public async Task SendMessage(string connectionId, string message)
	// {
	// 	await Clients.All.SendAsync("ReceiveMessage", connectionId, message);
	// }
	public async Task SendMessage(object messageObject)
	{
		await Clients.All.SendAsync("ReceiveMessage", messageObject);
	}
}
