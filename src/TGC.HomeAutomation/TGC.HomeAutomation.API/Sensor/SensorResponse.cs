namespace TGC.HomeAutomation.API.Sensor;

public record SensorResponse
{
	public string? Name { get; init; }
	public Guid Id { get; init; }
	public DateTime Created { get; init; }

	public static SensorResponse FromEntity(SensorEntity entity)
	{
		ArgumentNullException.ThrowIfNull(entity);
		return new SensorResponse { Name = entity.Name, Id = entity.Id, Created = entity.Created };
	}
}
