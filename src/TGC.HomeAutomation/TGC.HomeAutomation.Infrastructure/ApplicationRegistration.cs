using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using TGC.HomeAutomation.Application.Abstractions;
using TGC.HomeAutomation.Infrastructure.Cryptography;
using TGC.HomeAutomation.Infrastructure.InMemoryCache;
using TGC.HomeAutomation.Infrastructure.SignalR;

namespace TGC.HomeAutomation.Infrastructure;

public static class ApplicationRegistration
{
	public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
	{
		services.AddScoped<IDeviceCache, DeviceMacAddressCache>();
		
		services.AddScoped<IDeviceApiKeyGenerator, DeviceApiKeyGenerator>();
		
		services.AddSignalR();
		services.AddSingleton<ISignalRNotificationService, SignalRNotificationService>();
		
		return services;
	}

	public static IEndpointRouteBuilder UseInfrastructure(this IEndpointRouteBuilder app)
	{
		app.MapHub<AllClientsHub>("/signalr/all");
		return app;
	}
}