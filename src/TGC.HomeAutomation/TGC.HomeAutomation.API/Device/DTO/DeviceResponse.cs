namespace TGC.HomeAutomation.API.Sensor;

public record DeviceResponse
{
	public string? Name { get; init; }
	public string? MacAddress { get; init; }
	public DateTime Created { get; init; }
	public Guid Id { get; init; }

	public static DeviceResponse FromEntity(DeviceEntity entity)
	{
		ArgumentNullException.ThrowIfNull(entity);
		ArgumentNullException.ThrowIfNull(entity.RowKey);

		return new DeviceResponse { Name = entity.Name, MacAddress = entity.MacAddress, Created = entity.Created, Id = Guid.Parse(entity.RowKey) };
	}
}
