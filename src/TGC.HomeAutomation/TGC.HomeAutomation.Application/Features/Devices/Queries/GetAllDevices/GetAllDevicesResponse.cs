using TGC.HomeAutomation.Application.Abstractions;
using TGC.WebApi.Communication.Mediator;

namespace TGC.HomeAutomation.Application.Features.Devices.Queries.GetAllDevices;

public class GetAllDevicesResponse : BaseResponse, IQueryResponse
{
	public IEnumerable<DeviceDto> Devices { get; set; }
}

public class DeviceDto {
	public Guid Id { get; set; }
	public string? Name { get; set; }
	public string? MacAddress { get; set; }
	public DateTime Created { get; set; }
}