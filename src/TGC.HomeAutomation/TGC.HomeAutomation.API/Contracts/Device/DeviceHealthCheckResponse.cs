using TGC.HomeAutomation.Domain.Enums;

namespace TGC.HomeAutomation.API.Contracts.Device;

public class DeviceHealthCheckResponse
{
	public DeviceHealthCheckStatusEnum Status { get; set; }
	public string DeviceName { get; set; } = string.Empty;
	public string MacAddress { get; set; } = string.Empty;
	public Guid Id { get; set; }
	public DateTime LastActivity { get; set; }
}
