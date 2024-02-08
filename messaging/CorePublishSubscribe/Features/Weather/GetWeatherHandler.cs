using MediatR;
using Service.Contracts.Features.Weather;

namespace CorePublishSubscribe.Features.Weather;

public class GetWeatherHandler : IRequestHandler<GetWeatherQuery,WeatherResponse>
{
    private IRequestHandler<GetWeatherQuery, WeatherResponse> _requestHandlerImplementation;
    public Task<WeatherResponse> Handle(GetWeatherQuery request, CancellationToken cancellationToken)
    {
        return _requestHandlerImplementation.Handle(request, cancellationToken);
    }
}