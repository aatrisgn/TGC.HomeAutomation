using Microsoft.Extensions.DependencyInjection;
using TGC.HomeAutomation.Application.Abstractions;
using TGC.HomeAutomation.Application.Features.Devices.Commands.CreateDevice;
using TGC.HomeAutomation.Application.Features.Devices.Commands.DeleteDevice;
using TGC.HomeAutomation.Application.Features.Devices.Commands.UpsertApiKeyForDevice;
using TGC.HomeAutomation.Application.Features.Devices.Queries.GetAllDevices;
using TGC.HomeAutomation.Application.Features.Devices.Queries.GetDeviceById;
using TGC.HomeAutomation.Application.Features.Measures.Commands.CreateDeviceMeasure;
using TGC.HomeAutomation.Application.Features.Measures.Queries.GetMeasuresByDeviceId;
using TGC.HomeAutomation.Application.Services;
using TGC.WebApi.Communication.Mediator;

namespace TGC.HomeAutomation.Application;

public static class ApplicationRegistration
{
	public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
	{
		services.RegisterCommandHandlers();
		services.RegisterQueryHandlers();
		services.RegisterServices();
		return services;
	}
	
	private static IServiceCollection RegisterServices(this IServiceCollection services)
	{
		services.AddScoped<IDeviceLookup, DeviceLookup>();
		services.AddScoped<IApiKeyManager, ApiKeyManager>();
		return services;
	}

	private static IServiceCollection RegisterCommandHandlers(this IServiceCollection services)
	{
		services.AddScoped<ICommandHandler, CreateDeviceMeasureHandler>();
		services.AddScoped<ICommandHandler, UpsertApiKeyForDeviceHandler>();
		services.AddScoped<ICommandHandler, DeleteDeviceHandler>();
		services.AddScoped<ICommandHandler, CreateDeviceHandler>();
		
		return services;
	}
	
	private static IServiceCollection RegisterQueryHandlers(this IServiceCollection services)
	{
		services.AddScoped<IQueryHandler, GetAllDevicesQueryHandler>();
		services.AddScoped<IQueryHandler, GetDeviceByIdHandler>();
		services.AddScoped<IQueryHandler, GetMeasuresByDeviceIdHandler>();
		
		return services;
	}
}