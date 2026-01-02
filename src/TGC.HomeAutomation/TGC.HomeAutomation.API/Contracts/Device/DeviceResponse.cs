using TGC.HomeAutomation.API.Sensor;
using TGC.HomeAutomation.Application.Features.Devices.Queries.GetDeviceById;

namespace TGC.HomeAutomation.API.Contracts.Device;

public record DeviceResponse
{
	public string? Name { get; init; }
	public string? MacAddress { get; init; }
	public DateTime Created { get; init; }
	public Guid Id { get; init; }

	public static DeviceResponse FromEntity(DeviceEntity entity)
	{
		ArgumentNullException.ThrowIfNull(entity);
		ArgumentNullException.ThrowIfNull(entity.RowKey);

		return new DeviceResponse { Name = entity.Name, MacAddress = entity.MacAddress, Created = entity.Created, Id = Guid.Parse(entity.RowKey) };
	}

	public static DeviceResponse FromQueryResponse(GetDeviceByIdResponse response)
	{
		ArgumentNullException.ThrowIfNull(response);

		return new DeviceResponse
		{
			Created = response.Created,
			Id = response.Id,
			MacAddress = response.MacAddress,
			Name = response.Name
		};
	}
}
