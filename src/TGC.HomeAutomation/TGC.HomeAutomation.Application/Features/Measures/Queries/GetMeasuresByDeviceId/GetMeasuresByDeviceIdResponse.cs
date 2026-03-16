using TGC.HomeAutomation.Application.Abstractions;
using TGC.WebApi.Communication.Mediator;

namespace TGC.HomeAutomation.Application.Features.Measures.Queries.GetMeasuresByDeviceId;

public class GetMeasuresByDeviceIdResponse : BaseResponse, IQueryResponse
{
	public Guid DeviceId { get; set; }
	public IEnumerable<string> MeasureTypes { get; set; } = new List<string>();
}