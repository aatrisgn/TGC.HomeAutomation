using System.Text.Json.Serialization;

namespace TGC.OpenWeatherApi.Models;

public class WindResponse
{
	[JsonPropertyName("speed")]
	public double Speed { get; set; }

	[JsonPropertyName("deg")]
	public int Degree { get; set; }

	[JsonPropertyName("gust")]
	public double Gust { get; set; }
}