using System.Text.Json.Serialization;

namespace TGC.OpenWeatherApi.Models;

public class WeatherResponse
{
    [JsonPropertyName("coord")]
    public CoordinatesResponse Coordinates { get; set; }

    [JsonPropertyName("weather")]
    public List<WeatherDescriptionResponse> Weather { get; set; }

    [JsonPropertyName("base")]
    public string Base { get; set; }

    [JsonPropertyName("main")]
    public WeatherDetailsResponse WeatherDetails { get; set; }

    [JsonPropertyName("visibility")]
    public int Visibility { get; set; }

    [JsonPropertyName("wind")]
    public WindResponse WindResponse { get; set; }

    [JsonPropertyName("clouds")]
    public Clouds Clouds { get; set; }

    [JsonPropertyName("dt")]
    public long Dt { get; set; }

    [JsonPropertyName("sys")]
    public LocationDetailsResponse LocationDetails { get; set; }

    [JsonPropertyName("timezone")]
    public int Timezone { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("cod")]
    public int Cod { get; set; }
}