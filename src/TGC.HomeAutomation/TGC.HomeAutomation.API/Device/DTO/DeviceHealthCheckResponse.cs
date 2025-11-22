namespace TGC.HomeAutomation.API.Device.DTO;

public class DeviceHealthCheckResponse
{
	public DeviceHealthCheckStatusEnum Status { get; set; }
	public string DeviceName { get; set; } = string.Empty;
	public string MacAddress { get; set; } = string.Empty;
	public Guid Id { get; set; }
	public DateTime LastActivity { get; set; }
}
