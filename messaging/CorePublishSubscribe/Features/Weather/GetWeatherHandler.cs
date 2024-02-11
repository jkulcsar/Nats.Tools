using MediatR;
using Service.Contracts.Features.Weather;

namespace CorePublishSubscribe.Features.Weather;

public class GetWeatherHandler : IRequestHandler<GetWeatherQuery, Service.Contracts.Features.Weather.Weather>
{
    public Task<Service.Contracts.Features.Weather.Weather> Handle(GetWeatherQuery request, CancellationToken cancellationToken)
    {
        var weather = new Service.Contracts.Features.Weather.Weather();
        
        return Task.FromResult(weather);
    }
}