using Microsoft.Extensions.Primitives;

namespace TGC.HomeAutomation.API.Authentication;

public interface IAPIKeyRepository
{
	Task<StringValues> GetByDeviceIdAsync(StringValues deviceId);
}
