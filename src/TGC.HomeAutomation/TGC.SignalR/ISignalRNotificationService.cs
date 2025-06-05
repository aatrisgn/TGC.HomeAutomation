namespace TGC.SignalR;

public interface ISignalRNotificationService
{
	Task BroadcastMessageAsync<T>(T payload) where T : class;
}