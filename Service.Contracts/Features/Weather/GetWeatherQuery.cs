using MediatR;

namespace Service.Contracts.Features.Weather;

public class GetWeatherQuery(string city) : IRequest<Weather>
{
    
}