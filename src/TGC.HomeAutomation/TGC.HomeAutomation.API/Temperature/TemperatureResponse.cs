namespace TGC.HomeAutomation.API.Temperature;

public record TemperatureResponse
{
	public double Temperature { get; init; }
	public DateTime Created { get; init; }
	public string MacAddress { get; init; } = string.Empty;

	public static TemperatureResponse FromEntity(TemperatureEntity entity)
	{
		ArgumentNullException.ThrowIfNull(entity);
		return new TemperatureResponse { Temperature = entity.Temperature, MacAddress = entity.MacAddress, Created = entity.Created };
	}
}
