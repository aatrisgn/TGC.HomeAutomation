using TGC.AzureTableStorage;

namespace TGC.HomeAutomation.API.Measure;

[TableItem("Measures")]
public class OrderedMeasureEntity : AzureTableItem
{
	public string? Type { get; set; }
	public double DataValue { get; set; }
	public DateTime Created { get; set; }
	public Guid DeviceId { get; set; }
}
