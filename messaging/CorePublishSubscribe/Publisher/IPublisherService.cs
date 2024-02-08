using Service.Contracts.Features.Weather;

namespace CorePublishSubscribe.Publisher;

public interface IPublisherService
{
    IAsyncEnumerable<WeatherResponse?> PublishRandomMessages(int numberOfMessages);
}