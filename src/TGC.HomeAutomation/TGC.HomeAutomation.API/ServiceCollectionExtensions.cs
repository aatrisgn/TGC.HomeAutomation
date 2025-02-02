using Microsoft.FeatureManagement;
using TGC.AzureTableStorage.IoC;

namespace TGC.HomeAutomation.API;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddHomeAutomationApiInjections(this IServiceCollection services)
	{
		services.AddControllers();
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		services.AddEndpointsApiExplorer();
		services.AddSwaggerGen();

		services.AddFeatureManagement();

		services.AddAzureTableStorage(configuration =>
		{
			configuration.AccountConnectionString =
				"DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
			configuration.StubServices = false;
		});

		return services;
	}
}
