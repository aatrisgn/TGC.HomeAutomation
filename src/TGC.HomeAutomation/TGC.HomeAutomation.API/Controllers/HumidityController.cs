using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TGC.AzureTableStorage;
using TGC.HomeAutomation.API.Humidity;
using TGC.HomeAutomation.API.Temperature;

namespace TGC.HomeAutomation.API.Controllers;

public class HumidityController : HAControllerBase
{
	private readonly IAzureTableStorageRepository<HumidityEntity> _tableStorageRepository;

	public HumidityController(IAzureTableStorageRepository<HumidityEntity> tableStorageRepository)
	{
		_tableStorageRepository = tableStorageRepository;
	}

	[HttpGet]
	[Route("inside/current")]
	[ProducesResponseType(typeof(HumidityResponse), StatusCodes.Status200OK)]
	public async Task<HumidityResponse> GetCurrentInside()
	{
		DateTime dateTime = DateTime.UtcNow.AddDays(-7);
		var results = await _tableStorageRepository.GetAllAsync(t => t.Created >= dateTime);

		return results.Select(t => HumidityResponse.FromEntity(t)).First();
	}

	[HttpGet]
	[Route("outside/current")]
	[ProducesResponseType(typeof(HumidityResponse), StatusCodes.Status200OK)]
	public async Task<HumidityResponse> GetCurrentOutside()
	{
		DateTime dateTime = DateTime.UtcNow.AddDays(-7);
		var results = await _tableStorageRepository.GetAllAsync(t => t.Created >= dateTime);

		return results.Select(t => HumidityResponse.FromEntity(t)).First();
	}

	[HttpGet]
	[Route("inside/{startDate}/{endDate}")]
	[ProducesResponseType(typeof(HumidityResponse), StatusCodes.Status200OK)]
	public async Task<HumidityResponse> GetCurrentOutside(DateTime startDate, DateTime endDate)
	{
		DateTime dateTime = DateTime.UtcNow.AddDays(-7);
		var results = await _tableStorageRepository.GetAllAsync(t => t.Created >= dateTime);

		return results.Select(t => HumidityResponse.FromEntity(t)).First();
	}

	[HttpPost]
	[Authorize]
	[Route("inside")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<HumidityResponse> Create([FromBody] HumidityRequest request)
	{
		var newEntity = request.ToEntity();
		await _tableStorageRepository.CreateAsync(newEntity);
		return HumidityResponse.FromEntity(newEntity);
	}
}
