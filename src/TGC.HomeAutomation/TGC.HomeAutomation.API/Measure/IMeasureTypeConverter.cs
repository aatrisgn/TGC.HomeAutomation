namespace TGC.HomeAutomation.API.Measure;

public interface IMeasureTypeConverter
{
	Task<MeasureEntity> RequestToEntity(MeasureRequest measureRequest, Guid deviceId);
	Task<MeasureResponse> EntityToResponse(MeasureEntity measureEntity);
	MeasureType GetMeasureType(string measureType);
}
