using TGC.AzureTableStorage;

namespace TGC.HomeAutomation.API.Humidity;

[TableItem("Temperatures")]
public class HumidityEntity : AzureTableItem
{
	public double Humidity { get; set; }
	public Guid SensorId { get; set; }
	public DateTime Created { get; set; }
}
