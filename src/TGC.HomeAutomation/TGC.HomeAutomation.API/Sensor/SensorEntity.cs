using TGC.AzureTableStorage;

namespace TGC.HomeAutomation.API.Sensor;

[TableItem("Sensors")]
public class SensorEntity : AzureTableItem
{
	public string? Name { get; set; }
	public Guid Id { get; set; }
	public DateTime Created { get; set; }
}
