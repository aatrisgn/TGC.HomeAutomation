using TGC.HomeAutomation.Application.Abstractions;

namespace TGC.HomeAutomation.Application.Features.Devices.Queries.GetAllDevices;

public class GetAllDevicesResponse : IQueryResponse
{
	public IEnumerable<DeviceDto> Devices { get; set; }
}

public class DeviceDto {
	public Guid Id { get; set; }
	public string? Name { get; set; }
	public string? MacAddress { get; set; }
	public DateTime Created { get; set; }
}