using TGC.HomeAutomation.Application.Abstractions;

namespace TGC.HomeAutomation.Application.Features.Devices.Queries.GetDeviceById;

public class GetDeviceByIdQuery : IQuery
{
	public Guid DeviceId { get; }

	public GetDeviceByIdQuery(Guid id)
	{
		DeviceId = id;
	}
}