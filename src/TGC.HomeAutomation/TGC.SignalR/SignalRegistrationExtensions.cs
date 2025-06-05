using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace TGC.SignalR;

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