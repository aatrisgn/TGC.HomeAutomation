using TGC.HomeAutomation.Application.Abstractions;

namespace TGC.HomeAutomation.Application.Features.Devices.Commands.UpsertApiKeyForDevice;

public class UpsertApiKeyForDeviceHandler : BaseCommandHandler<UpsertApiKeyForDeviceCommand>, ICommandHandler
{
	private readonly IApiKeyManager _apiKeyManager;

	public UpsertApiKeyForDeviceHandler(IApiKeyManager apiKeyManager)
	{
		_apiKeyManager = apiKeyManager;
	}
	
	public async Task<ICommandResponse> Handle<TCommand>(TCommand command) where TCommand : ICommand
	{
		var castedCommand = command as UpsertApiKeyForDeviceCommand;
		
		var apiKeyResponse = await _apiKeyManager.UpsertApiKeyAsync(castedCommand);

		return apiKeyResponse;
	}
}