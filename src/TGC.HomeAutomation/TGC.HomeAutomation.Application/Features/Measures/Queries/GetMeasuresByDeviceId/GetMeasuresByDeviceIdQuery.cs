using TGC.HomeAutomation.Application.Abstractions;

namespace TGC.HomeAutomation.Application.Features.Measures.Queries.GetMeasuresByDeviceId;

public class GetMeasuresByDeviceIdQuery : BaseQuery
{
	public Guid DeviceId { get; }
	public GetMeasuresByDeviceIdQuery(Guid id)
	{
		DeviceId = id;
	}
}