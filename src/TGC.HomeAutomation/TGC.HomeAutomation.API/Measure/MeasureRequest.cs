namespace TGC.HomeAutomation.API.Measure;

public class MeasureRequest
{
	public double DataValue { get; init; }
	public string? Type { get; init; }
	public string? MacAddress { get; init; }
}
