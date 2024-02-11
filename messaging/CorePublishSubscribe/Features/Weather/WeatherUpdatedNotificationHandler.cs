using MediatR;
using Microsoft.Extensions.Logging;
using Service.Contracts.Features.Weather;

namespace CorePublishSubscribe.Features.Weather;


public class WeatherUpdatedNotificationHandler(ILogger<WeatherUpdatedNotificationHandler> logger) : INotificationHandler<WeatherNotification>
{
    public Task Handle(WeatherNotification weatherNotification, CancellationToken cancellationToken)
    {
        if (weatherNotification.Weather.OpenWeatherMapWeather != null)
            logger.LogInformation(
                $"Weather updated: {weatherNotification.Weather.Name} is now {weatherNotification.Weather.OpenWeatherMapWeather.Temp} degrees.");

        return Task.CompletedTask;
    }
}