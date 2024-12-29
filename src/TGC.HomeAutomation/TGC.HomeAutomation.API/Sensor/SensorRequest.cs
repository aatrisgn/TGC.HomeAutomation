using TGC.HomeAutomation.API.Sensor;

namespace TGC.HomeAutomation.API.Temperature;

public record SensorRequest
{
	public string Name { get; init; }

	public SensorRequest(string name)
	{
		Name = name;
	}

	public SensorEntity ToEntity()
	{
		return new SensorEntity { Name = Name, Id = Guid.NewGuid(), Created = DateTime.UtcNow };
	}
}
