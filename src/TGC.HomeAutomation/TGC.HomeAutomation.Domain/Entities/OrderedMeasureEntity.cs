using TGC.AzureTableStorage;

namespace TGC.HomeAutomation.API.Measure;

[TableItem("OrderedMeasures")]
public class OrderedMeasureEntity : AzureTableItem
{
	public string Type { get; set; } = "N/A";
	public double DataValue { get; set; }
	public DateTime Created { get; set; }
	public Guid DeviceId { get; set; }
	public int Sample { get; set; }
}
