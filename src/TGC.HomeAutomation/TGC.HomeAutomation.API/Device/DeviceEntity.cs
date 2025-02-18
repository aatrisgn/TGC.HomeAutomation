using TGC.AzureTableStorage;

namespace TGC.HomeAutomation.API.Sensor;

[TableItem("Devices")]
public class DeviceEntity : AzureTableItem
{
	public string? Name { get; set; }
	public string? MacAddress { get; set; }
	public DateTime Created { get; set; }
	public string? APIKey { get; set; }
}
