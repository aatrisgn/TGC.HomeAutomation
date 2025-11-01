using Azure.Monitor.OpenTelemetry.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.FeatureManagement;
using Microsoft.OpenApi.Models;
using TGC.AzureTableStorage.Configuration;
using TGC.AzureTableStorage.IoC;
using TGC.HomeAutomation.API.Authentication;
using TGC.HomeAutomation.API.Configuration;
using TGC.HomeAutomation.API.Device;
using TGC.HomeAutomation.API.Measure;
using TGC.HomeAutomation.API.SignalR;

namespace TGC.HomeAutomation.API;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddHomeAutomationApiInjections(this IServiceCollection services, IConfigurationManager configuration, IWebHostEnvironment environment)
	{
		if (!environment.IsDevelopment() && !environment.IsEnvironment("IntegrationTests"))
		{
			services.AddOpenTelemetry().UseAzureMonitor(options =>
			{
				options.ConnectionString = configuration.GetValue<string>("APPLICATIONINSIGHTS_CONNECTION_STRING");
			});
		}

		services.AddExceptionHandler<ApiExceptionHandler>();
		services.AddProblemDetails();

		var haConfigSection = configuration.GetSection(HomeAutomationConfiguration.SectionName);
		services.Configure<HomeAutomationConfiguration>(haConfigSection);

		var configSection = configuration.GetSection("TGC.AzureTableStorage");
		services.Configure<StorageConfiguration>(configSection);

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
		services.AddScoped<IOrderedMeasureService, OrderedMeasureService>();

		services.AddScoped<ICompositeMeasureService, CompositeMeasureService>();
		services.AddScoped<IMeasureTypeConverter, MeasureTypeConverter>();

		services.AddScoped<IDeviceCache, DeviceMacAddressCache>();

		var isTestDevicesEnabled = configuration.GetValue<bool>("HomeAutomation:Testing:Enabled");
		var testDeviceCount = configuration.GetValue<int>("HomeAutomation:Testing:TestDevices");

		if (isTestDevicesEnabled && testDeviceCount > 0)
		{
			services.AddSingleton(new FakeDeviceManager(testDeviceCount));
			services.AddHostedService<FakeDeviceBackgroundWorker>();
		}

		RegisterPortalJwtAuthentication(services, configuration);

		services.AddAuthentication(ApiKeyAuthSchemeOptions.DefaultScheme)
			.AddScheme<ApiKeyAuthSchemeOptions, ApiKeyAuthSchemeHandler>(
				ApiKeyAuthSchemeOptions.DefaultScheme,
				_ => { });

		services.AddAuthorization(options =>
			{
				options.FallbackPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
					.RequireAuthenticatedUser()
					.Build();
			});

		var useManagedIdentity = configuration.GetValue<bool>("TGC.AzureTableStorage:UseManagedIdentity");
		var connectionString = configuration.GetValue<string>("TGC.AzureTableStorage:ConnectionString");
		var storageAccountUrl = configuration.GetValue<string>("TGC.AzureTableStorage:StorageAccountUrl");
		var managedIdentityId = configuration.GetValue<string>("TGC.AzureTableStorage:ManagedIdentityId");

		services.AddAzureTableStorage(configuration =>
		{
			configuration.AccountConnectionString = connectionString;
			configuration.StorageAccountUrl = storageAccountUrl;
			configuration.UseManagedIdentity = useManagedIdentity;
			configuration.StubServices = false;
			configuration.ManagedIdentityId = string.IsNullOrEmpty(managedIdentityId) ? Guid.Empty : Guid.Parse(managedIdentityId);
		});

		var allowedHosts = configuration.GetSection("HomeAutomation:AllowedHosts").Get<string[]>();

		services.AddCors(options =>
		{
			options.AddPolicy(name: "CORS_ORIGINS_POLICY",
				builder =>
				{
					builder.WithOrigins(allowedHosts!)
						.AllowAnyMethod()
						.AllowAnyHeader()
						.AllowCredentials();
				});
		});

		services.AddControllers();

		services.AddHostedService<ConsolidationBackgroundWorker>();

		services.ConfigureSignalR();

		return services;
	}

	//Should be internal since used in implementation tests as well
	public static IServiceCollection RegisterPortalJwtAuthentication(this IServiceCollection services, IConfigurationManager configuration)
	{
		var clientId = configuration.GetSection("AzureAd:ClientId").Get<string>();
		var tenantId = configuration.GetSection("AzureAd:TenantId").Get<string>();
		var instance = configuration.GetSection("AzureAd:Instance").Get<string>();

		services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.Authority = $"{instance}{tenantId}/v2.0";
				options.Audience = clientId;

				options.Events = new JwtBearerEvents
				{
					OnMessageReceived = context =>
					{
						var accessToken = context.Request.Query["access_token"];
						var path = context.HttpContext.Request.Path;
						if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/signalr"))
						{
							context.Token = accessToken;
						}

						return Task.CompletedTask;
					}
				};
			});
		return services;
	}
}
