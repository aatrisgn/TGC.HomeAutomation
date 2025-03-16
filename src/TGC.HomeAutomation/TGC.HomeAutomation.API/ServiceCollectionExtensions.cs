using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using TGC.AzureTableStorage.IoC;
using TGC.HomeAutomation.API.Authentication;
using TGC.HomeAutomation.API.Device;
using TGC.HomeAutomation.API.Measure;

namespace TGC.HomeAutomation.API;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddHomeAutomationApiInjections(this IServiceCollection services, IConfiguration configuration)
	{
		services.AddOpenApiDocument(document =>
		{
			document.Title = "TGC.HomeAutomation API Spec";
			document.Description = "API for exposing data to Angular Client. Be aware this API is not ready for consumption by third-party. Breaking changes can occur without notice.";
		});

		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen(options =>
		{
			options.AddSecurityDefinition("x-device-api-key", new OpenApiSecurityScheme
			{
				Name = "x-device-api-key",
				In = ParameterLocation.Header,
				Description = "API key for device",
				Type = SecuritySchemeType.ApiKey
			});

			options.AddSecurityDefinition("x-device-id", new OpenApiSecurityScheme
			{
				Name = "x-device-id",
				In = ParameterLocation.Header,
				Description = "ID of device to authorize",
				Type = SecuritySchemeType.ApiKey
			});
			options.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
		});

		services.AddFeatureManagement();

		services.AddScoped<IDeviceAPIKeyGenerator, DeviceAPIKeyGenerator>();
		services.AddScoped<IDeviceService, DeviceService>();
		services.AddScoped<IAPIKeyRepository, MockAPIKeyRepository>();
		services.AddScoped<IOrderedMeasureService, OrderedMeasureService>();

		services.AddScoped<ICompositeMeasureService, CompositeMeasureService>();
		services.AddScoped<IMeasureTypeConverter, MeasureTypeConverter>();

		services.AddScoped<IDeviceCache, DeviceMacAddressCache>();

		services.AddHostedService<ConsolidationBackgroundWorker>();

		//Should be changed to default to Entra once that's implemented
		services.AddAuthentication(ApiKeyAuthSchemeOptions.DefaultScheme)
			.AddScheme<ApiKeyAuthSchemeOptions, ApiKeyAuthSchemeHandler>(
				ApiKeyAuthSchemeOptions.DefaultScheme,
				options => { });

		//TODO: Fix this way of passing configurations. Kinda bad.
		var some = configuration.GetSection("TGC.AzureTableStorage");

		var some2 = bool.TryParse(some["UseManagedIdentity"], out bool useManagedIdentity);

		services.AddAzureTableStorage(configuration =>
		{
			configuration.StorageAccountUrl = some["StorageAccountUrl"];
			configuration.UseManagedIdentity = useManagedIdentity;
			configuration.StubServices = false;
		});

		var allowedHosts = configuration.GetValue<string>("AllowedHosts");

		services.AddCors(options =>
		{
			options.AddPolicy(name: "ALLOW_DEVELOPMENT_CORS_ORIGINS_POLICY",
				builder =>
				{
					builder.WithOrigins(allowedHosts!)
						.AllowAnyMethod()
						.AllowAnyHeader();
				});
		});

		services.AddControllers();

		return services;
	}
}
