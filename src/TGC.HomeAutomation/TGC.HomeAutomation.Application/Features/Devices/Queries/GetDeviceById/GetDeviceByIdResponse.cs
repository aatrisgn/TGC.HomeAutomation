using TGC.HomeAutomation.API.Sensor;
using TGC.HomeAutomation.Application.Abstractions;

namespace TGC.HomeAutomation.Application.Features.Devices.Queries.GetDeviceById;

public class GetDeviceByIdResponse : IQueryResponse
{
	public string Name { get; init; }
	public string MacAddress { get; init; }
	public DateTime Created { get; init; }
	public Guid Id { get; init; }

	public static GetDeviceByIdResponse FromEntity(DeviceEntity locatedEntity)
	{
		return new GetDeviceByIdResponse
		{
			Created = locatedEntity.Created,
			Id = Guid.Parse(locatedEntity.RowKey),
			MacAddress = locatedEntity.MacAddress,
			Name = locatedEntity.Name
		};
	}
}