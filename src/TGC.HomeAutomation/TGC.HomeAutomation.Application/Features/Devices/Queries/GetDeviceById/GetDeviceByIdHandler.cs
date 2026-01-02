using TGC.HomeAutomation.Application.Abstractions;

namespace TGC.HomeAutomation.Application.Features.Devices.Queries.GetDeviceById;

public class GetDeviceByIdHandler : BaseQueryHandler<GetDeviceByIdQuery>, IQueryHandler
{
	private readonly IDeviceLookup _deviceLookup;
	
	public GetDeviceByIdHandler(IDeviceLookup deviceLookup)
	{
		_deviceLookup = deviceLookup;
	}
	
	public async Task<IQueryResponse> Handle<TQuery>(TQuery command) where TQuery : IQuery
	{
		var castedCommand = GetTypedQuery(command);
		var locatedEntity = await _deviceLookup.GetByIdAsync(castedCommand.DeviceId);
		return GetDeviceByIdResponse.FromEntity(locatedEntity);
	}
}