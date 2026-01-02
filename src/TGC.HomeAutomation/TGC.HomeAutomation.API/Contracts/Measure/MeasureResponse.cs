using TGC.HomeAutomation.Application.Features.Measures.Commands.CreateDeviceMeasure;

namespace TGC.HomeAutomation.API.Measure;

public class MeasureResponse
{
	public double DataValue { get; init; }
	public string? Type { get; init; }
	public DateTime Created { get; set; }
	public Guid DeviceId { get; set; }

	public static MeasureResponse FromCommandResult(CreateDeviceMeasureResponse createDeviceMeasureResponse)
	{
		return new MeasureResponse();
	}
}
