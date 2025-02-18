namespace TGC.HomeAutomation.API.Device;

public class DeviceAPIKey
{
	public string? APIKey { get; set; }
	public DateTime Expires { get; set; }
	public bool Expired => DateTime.UtcNow > Expires;
}
