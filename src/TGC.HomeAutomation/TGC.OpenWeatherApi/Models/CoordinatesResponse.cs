using System.Text.Json.Serialization;

namespace TGC.OpenWeatherApi.Models;

public class CoordinatesResponse
{
	[JsonPropertyName("lon")]
	public double Lon { get; set; }

	[JsonPropertyName("lat")]
	public double Lat { get; set; }
}