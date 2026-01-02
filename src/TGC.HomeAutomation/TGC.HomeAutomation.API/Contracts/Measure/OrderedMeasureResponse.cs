namespace TGC.HomeAutomation.API.Measure;

public class OrderedMeasureResponse
{
	public string? Type { get; set; }
	public double DataValue { get; set; }
	public DateTime Created { get; set; }
	public Guid DeviceId { get; set; }
	public int Sample { get; set; }
	public static OrderedMeasureResponse FromEntity(OrderedMeasureEntity orderedMeasureEntity)
	{
		return new OrderedMeasureResponse
		{
			Type = orderedMeasureEntity.Type,
			DeviceId = orderedMeasureEntity.DeviceId,
			Created = orderedMeasureEntity.Created,
			Sample = orderedMeasureEntity.Sample,
			DataValue = orderedMeasureEntity.DataValue
		};
	}
}
