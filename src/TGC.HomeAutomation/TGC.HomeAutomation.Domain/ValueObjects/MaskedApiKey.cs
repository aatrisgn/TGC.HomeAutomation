namespace TGC.HomeAutomation.API.Device;

public class MaskedApiKey
{
	public required string Secret { get; init; }
	public required byte[] Salt { get; init; }
}
