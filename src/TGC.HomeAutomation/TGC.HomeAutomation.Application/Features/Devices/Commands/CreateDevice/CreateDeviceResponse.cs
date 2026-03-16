using TGC.HomeAutomation.API.Sensor;
using TGC.HomeAutomation.Application.Abstractions;
using TGC.WebApi.Communication.Mediator;

namespace TGC.HomeAutomation.Application.Features.Devices.Commands.CreateDevice;

public class CreateDeviceResponse : BaseResponse, ICommandResponse
{
	public string? Name { get; init; }
	public string? MacAddress { get; init; }
	public DateTime Created { get; init; }
	public Guid Id { get; init; }

	public static CreateDeviceResponse FromEntity(DeviceEntity entity)
	{
		ArgumentNullException.ThrowIfNull(entity);
		ArgumentNullException.ThrowIfNull(entity.RowKey);

		return new CreateDeviceResponse
		{
			Name = entity.Name,
			MacAddress = entity.MacAddress,
			Created = entity.Created,
			Id = Guid.Parse(entity.RowKey)
		};
	}
}
