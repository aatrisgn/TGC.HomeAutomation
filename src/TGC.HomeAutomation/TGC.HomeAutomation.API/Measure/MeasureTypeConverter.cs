using TGC.HomeAutomation.Domain.Enums;

namespace TGC.HomeAutomation.API.Measure;

public class MeasureTypeConverter : IMeasureTypeConverter
{
	public MeasureType GetMeasureType(string measureType)
	{
		return (MeasureType)Enum.Parse(typeof(MeasureType), measureType, true);
	}
}
