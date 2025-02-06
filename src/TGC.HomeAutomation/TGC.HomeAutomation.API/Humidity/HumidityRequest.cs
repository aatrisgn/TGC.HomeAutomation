using TGC.HomeAutomation.API.Humidity;

namespace TGC.HomeAutomation.API.Temperature;

public record HumidityRequest
{
	public double Humidity { get; init; }
	public string? MacAddress { get; init; }

	public HumidityRequest()
	{
	}

	public HumidityRequest(float humidity)
	{
		Humidity = humidity;
	}

	public HumidityEntity ToEntity()
	{
		return new HumidityEntity { Humidity = Humidity, MacAddress = MacAddress ?? string.Empty, Created = DateTime.UtcNow };
	}
}
