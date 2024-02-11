using Microsoft.Extensions.Logging;
using NATS.Client.Core;
using Service.Contracts.Communication;
using Service.Contracts.Features.Weather;

namespace CorePublishSubscribe.Publisher;

public class SimplePublisherService(INatsConnection natsConnection, ILogger<SimplePublisherService> logger) : IPublisherService
{
    public async IAsyncEnumerable<Weather?> PublishRandomMessages(int numberOfMessages)
    {
        var cities = new[] {"London", "Paris", "New York", "Berlin", "Tokyo", "Sydney", "Beijing", "Moscow", "Cairo", "Rome"};
        var random = new Random();
        for (var i = 0; i < numberOfMessages; i++)
        {
            var city = cities[random.Next(cities.Length)];
            var weather = await GetWeatherForCity(city);
            logger.LogInformation($"OpenWeatherMapWeather in {weather.Name}: {weather.OpenWeatherMapWeather.Temp}°C");

            var weatherNotification = MessageEnvelope.CreateNotificationMessage(new WeatherNotification{ Weather = weather });
            await natsConnection.PublishAsync($"weather.{weather.Name}", weatherNotification);
            
            await Task.Delay(500);

            yield return weather;
        }
    }

    private Task<Weather> GetWeatherForCity(string city)
    {
        // generate random weather data for the city
        var random = new Random();
        var temp = random.Next(-20, 40);
        var feelsLike = random.Next(-20, 40);
        var tempMin = random.Next(-20, 40);
        var tempMax = random.Next(-20, 40);
        var pressure = random.Next(900, 1100);
        var humidity = random.Next(0, 100);
        
        return Task.FromResult(new Weather
        {
            OpenWeatherMapWeather = new OpenWeatherMapWeather
            {
                Temp = temp,
                FeelsLike = feelsLike,
                TempMin = tempMin,
                TempMax = tempMax,
                Pressure = pressure,
                Humidity = humidity
            },
            Visibility = 10000,
            Dt = 1630000000,
            Timezone = 3600,
            Id = 2643743,
            Name = city,
            Cod = 200
        });
            
    }
}