using Microsoft.Extensions.Primitives;

namespace TGC.HomeAutomation.API.Authentication;

public class MockAPIKeyRepository : IAPIKeyRepository
{
	public Task<StringValues> GetByDeviceIdAsync(StringValues deviceId)
	{
		return Task.FromResult(new StringValues("some"));
	}
}
