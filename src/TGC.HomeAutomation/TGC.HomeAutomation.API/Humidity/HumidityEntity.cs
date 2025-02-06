using TGC.AzureTableStorage;

namespace TGC.HomeAutomation.API.Humidity;

[TableItem("Temperatures")]
public class HumidityEntity : AzureTableItem
{
	public double Humidity { get; set; }
	public string MacAddress { get; init; } = string.Empty;
	public DateTime Created { get; set; }
}
