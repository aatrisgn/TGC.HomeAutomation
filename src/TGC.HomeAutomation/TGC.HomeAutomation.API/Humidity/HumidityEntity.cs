using TGC.AzureTableStorage;

namespace TGC.HomeAutomation.API.Humidity;

[TableItem("Temperatures")]
public class HumidityEntity : AzureTableItem
{
	public float Humidity { get; set; }
	public Guid SensorId { get; set; }
	public DateTime Created { get; set; }
}
