using Microsoft.AspNetCore.Mvc;
using TGC.AzureTableStorage;
using TGC.HomeAutomation.API.Temperature;

namespace TGC.HomeAutomation.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TemperatureController : ControllerBase
{
	private readonly IAzureTableStorageRepository<TemperatureEntity> _tableStorageRepository;

	public TemperatureController(IAzureTableStorageRepository<TemperatureEntity> tableStorageRepository)
	{
		_tableStorageRepository = tableStorageRepository;
	}

	[HttpGet]
	public async Task<IEnumerable<TemperatureResponse>> Get()
	{
		DateTime dateTime = DateTime.UtcNow.AddDays(-7);
		var results = await _tableStorageRepository.GetAllAsync(t => t.Created >= dateTime);

		return results.Select(t => TemperatureResponse.FromEntity(t));
	}

	[HttpPost]
	public async Task<TemperatureResponse> Create([FromBody] TemperatureRequest request)
	{
		var newEntity = request.ToEntity();
		await _tableStorageRepository.CreateAsync(newEntity);
		return TemperatureResponse.FromEntity(newEntity);
	}
}
