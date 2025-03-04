namespace TGC.HomeAutomation.API.Measure;

public class MeasureResponse
{
	public double DataValue { get; init; }
	public string? Type { get; init; }
	public string? MacAddress { get; init; }
	public DateTime Created { get; set; }
	public Guid DeviceId { get; set; }
}
