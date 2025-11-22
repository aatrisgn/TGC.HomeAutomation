using System.Text.Json.Serialization;

namespace TGC.OpenWeatherApi.Models;

public class Clouds
{
	[JsonPropertyName("all")]
	public int All { get; set; }
}