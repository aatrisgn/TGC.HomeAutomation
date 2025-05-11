namespace TGC.HomeAutomation.API.Configuration;

public class AuthConfiguration
{
	public string Instance { get; set; } = string.Empty;
	public string Domain { get; set; } = string.Empty;
	public Guid TenantId { get; set; }
	public Guid ClientId { get; set; }
}
