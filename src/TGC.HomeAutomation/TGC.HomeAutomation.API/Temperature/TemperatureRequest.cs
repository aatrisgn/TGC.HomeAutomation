namespace TGC.HomeAutomation.API.Temperature;

public record TemperatureRequest
{
	public float Temperature { get; init; }

	public TemperatureRequest(float temperature)
	{
		Temperature = temperature;
	}

	public TemperatureEntity ToEntity()
	{
		return new TemperatureEntity { Temperature = Temperature, Created = DateTime.UtcNow };
	}
}
