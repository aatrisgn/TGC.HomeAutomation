using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TGC.AzureTableStorage;
using TGC.HomeAutomation.API.Temperature;

namespace TGC.HomeAutomation.API.Controllers;

public class TemperatureController : HAControllerBase
{
	private readonly IAzureTableStorageRepository<TemperatureEntity> _tableStorageRepository;

	public TemperatureController(IAzureTableStorageRepository<TemperatureEntity> tableStorageRepository)
	{
		_tableStorageRepository = tableStorageRepository;
	}

	[HttpGet]
	[Route("inside/current")]
	[ProducesResponseType(typeof(TemperatureResponse), StatusCodes.Status200OK)]
	public async Task<TemperatureResponse> GetCurrentInside()
	{
		DateTime dateTime = DateTime.UtcNow.AddDays(-1);
		var results = await _tableStorageRepository.GetAllAsync(t => t.Created >= dateTime);

		return results.Select(t => TemperatureResponse.FromEntity(t)).OrderBy(t => t.Created).First();
	}

	[HttpGet]
	[Route("outside/current")]
	[ProducesResponseType(typeof(TemperatureResponse), StatusCodes.Status200OK)]
	public async Task<TemperatureResponse> GetCurrentOutside()
	{
		DateTime dateTime = DateTime.UtcNow.AddDays(-7);
		var results = await _tableStorageRepository.GetAllAsync(t => t.Created >= dateTime);

		return results.Select(t => TemperatureResponse.FromEntity(t)).First();
	}

	[HttpGet]
	[Route("inside/{startDate}/{endDate}")]
	[ProducesResponseType(typeof(IEnumerable<TemperatureResponse>), StatusCodes.Status200OK)]
	public async Task<IEnumerable<TemperatureResponse>> GetCurrentOutside(DateTime startDate, DateTime endDate)
	{
		var results = await _tableStorageRepository.GetAllAsync(t => t.Created >= startDate && t.Created <= endDate);

		return results.Select(t => TemperatureResponse.FromEntity(t));
	}

	[HttpPost]
	[Authorize]
	[Route("inside")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<TemperatureResponse> Create([FromBody] TemperatureRequest request)
	{
		var newEntity = request.ToEntity();
		await _tableStorageRepository.CreateAsync(newEntity);
		return TemperatureResponse.FromEntity(newEntity);
	}
}
