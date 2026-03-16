using TGC.HomeAutomation.Application.Abstractions;
using TGC.WebApi.Communication.Mediator;

namespace TGC.HomeAutomation.Application.Features.Devices.Queries.GetDeviceById;

public class GetDeviceByIdHandler : BaseQueryHandler<GetDeviceByIdQuery>, IQueryHandler
{
	private readonly IDeviceLookup _deviceLookup;
	
	public GetDeviceByIdHandler(IDeviceLookup deviceLookup)
	{
		_deviceLookup = deviceLookup;
	}
	
	public async Task<IResult<IQueryResponse>> Handle<TQuery>(TQuery command) where TQuery : IQuery
	{
		var castedCommand = GetTypedQuery(command);
		var locatedEntity = await _deviceLookup.GetByIdAsync(castedCommand.DeviceId);
		return Result<GetDeviceByIdResponse>.Success(GetDeviceByIdResponse.FromEntity(locatedEntity));
	}
}