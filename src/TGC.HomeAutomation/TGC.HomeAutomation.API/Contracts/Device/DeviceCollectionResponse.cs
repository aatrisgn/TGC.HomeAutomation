using TGC.HomeAutomation.Application.Features.Devices.Queries.GetAllDevices;

namespace TGC.HomeAutomation.API.Contracts.Device;

public class DeviceCollectionResponse
{
	public IEnumerable<DeviceResponse> Devices { get; set; } = [];

	public static DeviceCollectionResponse FromQueryResponse(GetAllDevicesResponse result)
	{
		return new DeviceCollectionResponse()
		{
			Devices = result.Devices.Select(d =>
			{
				return new DeviceResponse { Created = d.Created, Id = d.Id, Name = d.Name, MacAddress = d.MacAddress };
			})
		};
	}
}
