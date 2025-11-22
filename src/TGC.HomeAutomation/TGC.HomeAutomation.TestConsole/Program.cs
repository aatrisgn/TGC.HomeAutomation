using Microsoft.Extensions.DependencyInjection;
using TGC.OpenWeatherApi;

Console.WriteLine("Starting application...");

var serviceCollection = new ServiceCollection();

serviceCollection.AddOpenWeatherApi(options =>
{
	options.KeyvaultUrl = "";
	options.KeyvaultSecretName = "";
	options.UseKeyvault = true;
});

var serviceProvider = serviceCollection.BuildServiceProvider();

var openWeatherApiClient = serviceProvider.GetService<IOpenWeatherApiClient>();

var some = openWeatherApiClient.GetCurrentWeatherAsync("55", "11").Result;

Console.WriteLine(some);