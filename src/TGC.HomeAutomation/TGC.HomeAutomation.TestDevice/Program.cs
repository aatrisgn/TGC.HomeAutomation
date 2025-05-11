using System.Net.Http.Json;
using TGC.HomeAutomation.API.Measure;
using TGC.HomeAutomation.API.Temperature;

Console.WriteLine("Hello, World!");

var apiKey = Environment.GetEnvironmentVariable("API_KEY");
var deviceId = Environment.GetEnvironmentVariable("DEVICE_ID");

var maxRead = 25;
var minRead = 18;

Random random = new Random();

var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://localhost:5298");

var readsToSend = 15;
var counter = 0;

while (counter < readsToSend)
{
	double randomNumber = random.NextDouble() * (maxRead - minRead) + minRead;
	var randomRoundedNumber = Math.Round(randomNumber, 1);

	await httpClient.PostAsJsonAsync("/api/measure/inside", new MeasureRequest
	{
		DataValue = randomRoundedNumber,
		Type = "co2",
		MacAddress = "00-B0-D0-63-C2-25"
	});

	counter++;
}