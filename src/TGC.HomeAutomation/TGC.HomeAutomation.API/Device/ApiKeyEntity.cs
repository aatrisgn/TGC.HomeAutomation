using TGC.AzureTableStorage;

namespace TGC.HomeAutomation.API.Device;

[TableItem("ApiKeys")]
public class ApiKeyEntity : AzureTableItem
{
	public string Secret { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public byte[] Salt { get; set; } = [];
	public DateTime ExpirationDate { get; set; }
	public Guid DeviceId { get; set; }
	public bool Expired => DateTime.UtcNow > ExpirationDate;
}
