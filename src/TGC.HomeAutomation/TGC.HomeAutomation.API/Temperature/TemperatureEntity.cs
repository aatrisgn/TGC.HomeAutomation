using TGC.AzureTableStorage;

namespace TGC.HomeAutomation.API.Temperature;

[TableItem("Temperatures")]
public class TemperatureEntity : AzureTableItem
{
	public double Temperature { get; set; }
	public Guid SensorId { get; set; }
	public DateTime Created { get; set; }
}
