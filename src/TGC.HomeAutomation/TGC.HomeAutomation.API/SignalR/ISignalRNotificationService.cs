namespace TGC.HomeAutomation.API.SignalR;

public interface ISignalRNotificationService
{
	Task BroadcastMessageAsync<T>(T payload) where T : class;
}
