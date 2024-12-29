namespace TGC.HomeAutomation.API.Temperature;

public record TemperatureResponse
{
	public float Temperature { get; init; }
	public DateTime Created { get; init; }

	public static TemperatureResponse FromEntity(TemperatureEntity entity)
	{
		ArgumentNullException.ThrowIfNull(entity);
		return new TemperatureResponse { Temperature = entity.Temperature, Created = entity.Created };
	}
}
