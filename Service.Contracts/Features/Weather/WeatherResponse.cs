using System.Text.Json.Serialization;

namespace Service.Contracts.Features.Weather;

public class WeatherResponse
{
    [JsonPropertyName("main")] public OpenWeatherMapWeather Weather { get; set; }

    [JsonPropertyName("visibility")] public int Visibility { get; set; }

    [JsonPropertyName("dt")] public int Dt { get; set; }

    [JsonPropertyName("timezone")] public int Timezone { get; set; }

    [JsonPropertyName("id")] public int Id { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("cod")] public int Cod { get; set; }
}