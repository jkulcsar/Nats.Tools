using MediatR;

namespace Service.Contracts.Features.Weather;

public class WeatherNotification : INotification
{
    public Weather Weather { get; init; }
}