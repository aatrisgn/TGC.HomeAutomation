using TGC.HomeAutomation.API.Measure;
using TGC.HomeAutomation.Application.Abstractions;

namespace TGC.HomeAutomation.Application.Features.Measures.Commands.CreateDeviceMeasure;

public class CreateDeviceMeasureCommand : ICommand
{
	public double DataValue { get; init; }
	public string? Type { get; init; }
	public string? MacAddress { get; init; }
	
	public MeasureEntity ToMeasureEntity(Guid deviceId, DateTime created)
	{
		var measureEnity = new MeasureEntity
		{
			Created = created,
			Type = Type!.ToString(),
			DataValue = DataValue,
			DeviceId = deviceId
		};
		
		return measureEnity;
	}
}