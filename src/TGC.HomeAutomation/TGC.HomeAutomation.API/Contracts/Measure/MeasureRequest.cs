using TGC.HomeAutomation.Application.Features.Measures.Commands.CreateDeviceMeasure;

namespace TGC.HomeAutomation.API.Measure;

public record MeasureRequest
{
	public double DataValue { get; init; }
	public string? Type { get; init; }
	public string? MacAddress { get; init; }

	public CreateDeviceMeasureCommand ToCommand()
	{
		return new CreateDeviceMeasureCommand
		{
			DataValue = DataValue,
			Type = Type,
			MacAddress = MacAddress
		};
	}
}
