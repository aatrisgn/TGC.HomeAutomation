using TGC.HomeAutomation.Application.Features.Measures.Queries.GetMeasuresByDeviceId;

namespace TGC.HomeAutomation.API.Device;

public class DeviceMeasureTypesResponse
{
	public Guid DeviceId { get; set; }
	public IEnumerable<string> MeasureTypes { get; set; } = new List<string>();

	public static DeviceMeasureTypesResponse FromQueryResponse(GetMeasuresByDeviceIdResponse queryResponse)
	{
		return new DeviceMeasureTypesResponse
		{
			DeviceId = queryResponse.DeviceId,
			MeasureTypes = queryResponse.MeasureTypes
		};
	}
}
