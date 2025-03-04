namespace TGC.HomeAutomation.API.Measure;

public class MeasureTypeConverter : IMeasureTypeConverter
{
	private readonly TimeProvider _timeProvider;

	public MeasureTypeConverter(TimeProvider timeProvider)
	{
		_timeProvider = timeProvider;
	}

	public Task<MeasureEntity> RequestToEntity(MeasureRequest measureRequest, Guid deviceId)
	{
		var measureEnity = new MeasureEntity
		{
			Created = _timeProvider.GetUtcNow().UtcDateTime,
			Type = measureRequest.Type!.ToString(),
			DataValue = measureRequest.DataValue,
			DeviceId = deviceId
		};

		return Task.FromResult(measureEnity);
	}

	public Task<MeasureResponse> EntityToResponse(MeasureEntity measureEntity)
	{
		throw new NotImplementedException();
	}

	public MeasureType GetMeasureType(string measureType)
	{
		return (MeasureType)Enum.Parse(typeof(MeasureType), measureType, true);
	}
}
