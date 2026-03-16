using TGC.HomeAutomation.Application.Abstractions;

namespace TGC.HomeAutomation.Application.Features.Devices.Queries.GetAllDevices;

public class GetAllDevicesQuery : BaseQuery
{
	public static GetAllDevicesQuery Empty()
	{
		return new GetAllDevicesQuery();
	}
}