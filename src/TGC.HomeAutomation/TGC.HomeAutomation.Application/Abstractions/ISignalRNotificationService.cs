namespace TGC.HomeAutomation.Application.Abstractions;

public interface ISignalRNotificationService
{
	Task BroadcastMessageAsync<T>(T payload) where T : class;
}
