using TGC.HomeAutomation.API.Sensor;

namespace TGC.HomeAutomation.API.Temperature;

public record DeviceRequest
{
	public string Name { get; init; }
	public string MacAdrress { get; init; }

	public DeviceRequest(string name, string macAdrress)
	{
		Name = name;
		MacAdrress = macAdrress;
	}

	public DeviceEntity ToEntity()
	{
		return new DeviceEntity { Name = Name, MacAddress = MacAdrress, Created = DateTime.UtcNow };
	}
}
