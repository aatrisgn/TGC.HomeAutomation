using TGC.HomeAutomation.API.Sensor;

namespace TGC.HomeAutomation.API.Temperature;

public record DeviceRequest
{
	public string? Name { get; set; }
	public string? MacAddress { get; set; }

	public DeviceEntity ToEntity()
	{
		return new DeviceEntity { Name = Name, MacAddress = MacAddress, Created = DateTime.UtcNow };
	}
}
