namespace TGC.HomeAutomation.API.Device;

public class DeviceMeasureTypesResponse
{
	public Guid DeviceId { get; set; }
	public IEnumerable<string> MeasureTypes { get; set; } = new List<string>();
}
