using TGC.HomeAutomation.Application.Abstractions;

namespace TGC.HomeAutomation.Application.Features.Devices.Queries.GetAllDevices;

public class GetAllDevicesQueryHandler : BaseQueryHandler<GetAllDevicesQuery>, IQueryHandler
{
	private readonly IDeviceLookup _deviceLookup;
	
	public GetAllDevicesQueryHandler(IDeviceLookup deviceLookup)
	{
		_deviceLookup = deviceLookup;
	}
	
	public async Task<IQueryResponse> Handle<TQuery>(TQuery command)
		where TQuery : IQuery
	{
		var allDevices = await _deviceLookup.GetAllAsync();
		var deviceDtos = allDevices.Select(device => new DeviceDto
		{
			Id = Guid.Parse(device.RowKey),
			Name = device.Name,
			MacAddress = device.MacAddress,
			Created = device.Created
		});
		
		var response = new GetAllDevicesResponse { Devices = deviceDtos };

		return response;
	}
}