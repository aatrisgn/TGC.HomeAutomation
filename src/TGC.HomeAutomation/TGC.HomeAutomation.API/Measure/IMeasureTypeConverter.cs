using TGC.HomeAutomation.Domain.Enums;

namespace TGC.HomeAutomation.API.Measure;

public interface IMeasureTypeConverter
{
	MeasureType GetMeasureType(string measureType);
}
