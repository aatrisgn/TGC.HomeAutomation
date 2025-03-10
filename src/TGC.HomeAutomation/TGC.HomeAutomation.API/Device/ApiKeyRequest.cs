namespace TGC.HomeAutomation.API.Device;

public class ApiKeyRequest
{
	public DateOnly ExpirationDate { get; set; }
	public required string Name { get; set; }
}
