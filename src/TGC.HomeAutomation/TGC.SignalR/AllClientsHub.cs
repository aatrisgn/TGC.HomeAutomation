using Microsoft.AspNetCore.SignalR;

namespace TGC.SignalR;

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