namespace TGC.HomeAutomation.API.Device;

public class ApiKeyResponse
{
	public DateOnly ExpirationDate { get; set; }
	public required string Name { get; set; }
	public required string Secret { get; set; }
}
