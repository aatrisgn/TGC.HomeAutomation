namespace TGC.HomeAutomation.API.Measure;

public class DeviceOrderedMeasureRangeResponse
{
	public Guid DeviceId { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public IEnumerable<OrderedMeasureResponse> Measures { get; set; } = [];
}
