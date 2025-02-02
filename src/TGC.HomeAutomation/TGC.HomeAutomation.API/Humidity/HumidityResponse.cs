using TGC.HomeAutomation.API.Humidity;

namespace TGC.HomeAutomation.API.Temperature;

public record HumidityResponse
{
	public double Humidity { get; init; }
	public Guid SensorId { get; init; }
	public DateTime Created { get; init; }

	public static HumidityResponse FromEntity(HumidityEntity entity)
	{
		ArgumentNullException.ThrowIfNull(entity);
		return new HumidityResponse { Humidity = entity.Humidity, SensorId = entity.SensorId, Created = entity.Created };
	}
}
