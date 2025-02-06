namespace TGC.HomeAutomation.API.Temperature;

public record TemperatureRequest
{
	public double Temperature { get; init; }
	public string? MacAddress { get; init; }

	public TemperatureRequest()
	{
	}

	public TemperatureRequest(float temperature)
	{
		Temperature = temperature;
	}

	public TemperatureEntity ToEntity()
	{
		return new TemperatureEntity { Temperature = Temperature, MacAddress = MacAddress ?? string.Empty, Created = DateTime.UtcNow };
	}
}
