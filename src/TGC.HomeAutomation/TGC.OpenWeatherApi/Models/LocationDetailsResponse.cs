using System.Text.Json.Serialization;

namespace TGC.OpenWeatherApi.Models;

public class LocationDetailsResponse
{
	[JsonPropertyName("country")]
	public string Country { get; set; }

	[JsonPropertyName("sunrise")]
	public long Sunrise { get; set; }

	[JsonPropertyName("sunset")]
	public long Sunset { get; set; }
}