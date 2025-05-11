namespace TGC.HomeAutomation.API.Device;

public class FakeDeviceManager
{
	private List<FakeDataDevice> FakeDataDevices = [];
	public FakeDeviceManager(int fakeDeviceCount)
	{
		for (int i = 0; i < fakeDeviceCount; i++)
		{
			FakeDataDevices.Add(new FakeDataDevice
			{
				MacAddress = GenerateRandomMacAddress()
			});
		}
	}
	public async Task<IEnumerable<FakeDataDevice>> GetFakeDevices()
	{
		return FakeDataDevices;
	}

	private static string GenerateRandomMacAddress()
	{
		Random rand = new Random();
		byte[] macBytes = new byte[6];
		rand.NextBytes(macBytes);

		// Set the locally administered bit (bit 1 of the first byte)
		macBytes[0] = (byte)((macBytes[0] & 0xFE) | 0x02);

		return BitConverter.ToString(macBytes).Replace("-", ":");
	}
}
