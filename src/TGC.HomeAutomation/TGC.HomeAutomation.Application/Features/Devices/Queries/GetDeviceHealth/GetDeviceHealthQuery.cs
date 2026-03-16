using TGC.HomeAutomation.Application.Abstractions;

namespace TGC.HomeAutomation.Application.Features.Devices.Queries.GetDeviceHealth;

public class GetDeviceHealthQuery : BaseQuery
{
	public Guid DeviceId { get; }
	public GetDeviceHealthQuery(Guid id)
	{
		DeviceId = id;
	}
}