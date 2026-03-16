using TGC.HomeAutomation.Application.Abstractions;
using TGC.HomeAutomation.Application.Features.Devices.Queries.GetDeviceHealth;
using TGC.WebApi.Communication.Mediator;

namespace TGC.HomeAutomation.Application.Features.Measures.Queries.GetDeviceHealth;

public class GetDeviceHealthHandler : BaseQueryHandler<GetDeviceHealthQuery>, IQueryHandler
{
	private readonly IDeviceLookup _deviceLookup;
	
	public GetDeviceHealthHandler(IDeviceLookup deviceLookup)
	{
		_deviceLookup = deviceLookup;
	}
	
	public async Task<IResult<IQueryResponse>> Handle<TQuery>(TQuery command) where TQuery : IQuery
	{
		var castedCommand = GetTypedQuery(command);
		
		var device = await _deviceLookup.GetByIdAsync(castedCommand.DeviceId);

		if (device is null)
		{
			return Result<GetDeviceHealthResponse>.Failure("Not found");
		}
		
		
		throw new NotImplementedException();
	}
}