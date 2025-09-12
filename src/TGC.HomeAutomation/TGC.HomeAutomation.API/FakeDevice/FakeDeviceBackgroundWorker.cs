using TGC.HomeAutomation.API.Measure;
using TGC.HomeAutomation.API.Temperature;

namespace TGC.HomeAutomation.API.Device;

public class FakeDeviceBackgroundWorker : BackgroundService
{
	private readonly ILogger<FakeDeviceBackgroundWorker> _logger;
	private readonly IServiceProvider _services;

	private Random _random;

	public FakeDeviceBackgroundWorker(IServiceProvider services, ILogger<FakeDeviceBackgroundWorker> logger)
	{
		_logger = logger;
		_services = services;
		_random = new Random();
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("Consolidating measures...");

		// When the timer should have no due-time, then do the work once now.
		await GenerateTestData();

		using PeriodicTimer timer = new(TimeSpan.FromSeconds(30));

		try
		{
			while (await timer.WaitForNextTickAsync(stoppingToken))
			{
				await GenerateTestData();
			}
		}
		catch (OperationCanceledException)
		{
			_logger.LogInformation("Timed Hosted Service is stopping.");
		}
	}

	private async Task GenerateTestData()
	{
		using var scope = _services.CreateScope();
		var fakeDeviceManager = scope.ServiceProvider.GetRequiredService<FakeDeviceManager>();
		var fakeDevices = await fakeDeviceManager.GetFakeDevices();

		foreach (var fakeDevice in fakeDevices)
		{
			_logger.LogInformation("I am {MacAddress} ({DeviceName})", fakeDevice.MacAddress, fakeDevice.Name);

			if (fakeDevice.Id == Guid.Empty)
			{
				var deviceService = scope.ServiceProvider.GetRequiredService<IDeviceService>();

				var newDeviceRequest = new DeviceRequest { Name = fakeDevice.Name, MacAddress = fakeDevice.MacAddress, };

				var newTestDevice = await deviceService.CreateAsync(newDeviceRequest);
				var deviceAPIKey = await deviceService.UpsertApiKeyAsync(
					new ApiKeyRequest
					{
						Name = fakeDevice.Name,
						ExpirationDate = DateOnly.FromDateTime(DateTime.Now.AddDays(30))
					}, newTestDevice.Id);

				fakeDevice.Id = newTestDevice.Id;
				fakeDevice.ApiKey = deviceAPIKey.Secret;
			}

			using var httpClient = new HttpClient();
			httpClient.BaseAddress = new Uri("http://localhost:5298");

			httpClient.DefaultRequestHeaders.Add("x-device-api-key", fakeDevice.ApiKey);
			httpClient.DefaultRequestHeaders.Add("x-device-id", fakeDevice.Id.ToString());

			var result = await httpClient.PostAsJsonAsync("/api/measure/inside", GenerateCo2Payload(fakeDevice));
			await httpClient.PostAsJsonAsync("/api/measure/inside", GenerateTemperaturePayload(fakeDevice));
			await httpClient.PostAsJsonAsync("/api/measure/inside", GenerateHumidityPayload(fakeDevice));
		}
	}

	public MeasureRequest GenerateCo2Payload(FakeDataDevice fakeDevice)
	{
		var maxRead = 50;
		var minRead = 20;

		double randomNumber = _random.NextDouble() * (maxRead - minRead) + minRead;
		var randomRoundedNumber = Math.Round(randomNumber, 1);

		return new MeasureRequest() { DataValue = randomRoundedNumber, Type = "co2", MacAddress = fakeDevice.MacAddress };
	}

	public MeasureRequest GenerateTemperaturePayload(FakeDataDevice fakeDevice)
	{
		var maxRead = 25;
		var minRead = 18;

		double randomNumber = _random.NextDouble() * (maxRead - minRead) + minRead;
		var randomRoundedNumber = Math.Round(randomNumber, 1);

		return new MeasureRequest() { DataValue = randomRoundedNumber, Type = "temperature", MacAddress = fakeDevice.MacAddress };
	}

	public MeasureRequest GenerateHumidityPayload(FakeDataDevice fakeDevice)
	{
		var maxRead = 100;
		var minRead = 1;

		double randomNumber = _random.NextDouble() * (maxRead - minRead) + minRead;
		var randomRoundedNumber = Math.Round(randomNumber, 1);

		return new MeasureRequest() { DataValue = randomRoundedNumber, Type = "humidity", MacAddress = fakeDevice.MacAddress };
	}

	private static string GenerateRandomMacAddress()
	{
		Random rand = new Random();
		byte[] macBytes = new byte[6];
		rand.NextBytes(macBytes);

		// Set the locally administered bit (bit 1 of the first byte)
		macBytes[0] = (byte)((macBytes[0] & 0xFE) | 0x02);

		return BitConverter.ToString(macBytes).Replace("-", ":");
	}
}
