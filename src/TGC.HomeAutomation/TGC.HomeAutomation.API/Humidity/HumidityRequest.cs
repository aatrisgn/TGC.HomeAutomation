using TGC.HomeAutomation.API.Humidity;

namespace TGC.HomeAutomation.API.Temperature;

public record HumidityRequest
{
	public double Humidity { get; init; }
	public Guid SensorId { get; init; }

	public HumidityRequest(float humidity)
	{
		Humidity = humidity;
	}

	public HumidityEntity ToEntity()
	{
		return new HumidityEntity { Humidity = Humidity, SensorId = SensorId, Created = DateTime.UtcNow };
	}
}
