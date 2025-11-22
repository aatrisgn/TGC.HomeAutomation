using Microsoft.Extensions.DependencyInjection;
using TGC.OpenWeatherApi.Test;

namespace TGC.OpenWeatherApi;

public static class IServiceCollectionExtensions
{
	public static IServiceCollection AddOpenWeatherApi(this IServiceCollection services, Action<OpenWeatherApiConfiguration> configuration)
	{
		ArgumentNullException.ThrowIfNull(configuration);
		
		var openWeatherApiConfiguration = new OpenWeatherApiConfiguration();
		
		configuration.Invoke(openWeatherApiConfiguration);
		
		services.ConfigureComponent(openWeatherApiConfiguration);
		
		return services;
	}

	public static IServiceCollection AddOpenWeatherApi(this IServiceCollection services,
		IOpenWeatherApiConfiguration configuration)
	{
		services.ConfigureComponent(configuration);
		return services;
	}

	private static IServiceCollection ConfigureComponent(this IServiceCollection services, IOpenWeatherApiConfiguration configuration)
	{
		services.AddSingleton<IOpenWeatherApiConfiguration>(configuration);

		if (configuration.Mock)
		{
			services.AddScoped<IOpenWeatherApiClient, MockOpenWeatherApiClient>();
		}
		else
		{
			services.AddSingleton<IKeyvaultReaderService>(new KeyvaultReaderService(configuration));
			services.AddScoped<IOpenWeatherApiClient, OpenWeatherApiClient>();

			services.AddHttpClient<IOpenWeatherApiClient, OpenWeatherApiClient>(client =>
			{
				client.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5/");
			});
		}
		
		return services;
	}
}