namespace TGC.HomeAutomation.API.Device;

public class FakeDataDevice
{
	public required string MacAddress { get; set; }
	public string Name => "TEST_DEVICE_" + MacAddress;
	public Guid Id { get; set; }
}
