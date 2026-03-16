using TGC.AzureTableStorage;
using TGC.HomeAutomation.API.Measure;
using TGC.HomeAutomation.Application.Abstractions;
using TGC.WebApi.Communication.Mediator;

namespace TGC.HomeAutomation.Application.Features.Measures.Queries.GetMeasuresByDeviceId;

public class GetMeasuresByDeviceIdHandler : BaseQueryHandler<GetMeasuresByDeviceIdQuery>, IQueryHandler
{
	private readonly IAzureTableStorageRepository<OrderedMeasureEntity> _orderedMeasureRepository;
	
	public GetMeasuresByDeviceIdHandler(IAzureTableStorageRepository<OrderedMeasureEntity> orderedMeasureRepository)
	{
		_orderedMeasureRepository = orderedMeasureRepository;
	}

	public async Task<IResult<IQueryResponse>> Handle<TQuery>(TQuery command) where TQuery : IQuery
	{
		var query = GetTypedQuery(command);
		var allOrderedMeasures = await _orderedMeasureRepository.GetAllAsync(d => d.DeviceId == query.DeviceId);

		if (allOrderedMeasures is null || !allOrderedMeasures.Any())
		{
			var emptyResult = new GetMeasuresByDeviceIdResponse { DeviceId = query.DeviceId, MeasureTypes = [], StatusCode = System.Net.HttpStatusCode.OK };
			return Result<GetMeasuresByDeviceIdResponse>.Success(emptyResult);
		}

		var distinctValues = allOrderedMeasures.Select(m => m.Type);

		distinctValues = distinctValues.Distinct() ?? [];
		
		var response = new GetMeasuresByDeviceIdResponse { DeviceId = query.DeviceId, MeasureTypes = distinctValues, StatusCode = System.Net.HttpStatusCode.OK };

		return Result<GetMeasuresByDeviceIdResponse>.Success(response);
	}
}