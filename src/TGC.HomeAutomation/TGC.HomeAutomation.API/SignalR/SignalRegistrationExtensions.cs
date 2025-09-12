namespace TGC.HomeAutomation.API.SignalR;

public static class SignalRegistrationExtensions
{
	public static IServiceCollection ConfigureSignalR(this IServiceCollection services)
	{
		services.AddSignalR();
		services.AddSingleton<ISignalRNotificationService, SignalRNotificationService>();
		return services;
	}

	public static IEndpointRouteBuilder UseSignalR(this IEndpointRouteBuilder app)
	{
		app.MapHub<AllClientsHub>("/signalr/all");
		return app;
	}
}
