using Service.Contracts.Features.Weather;

namespace CorePublishSubscribe.Publisher;

public interface IPublisherService
{
    IAsyncEnumerable<Weather?> PublishRandomMessages(int numberOfMessages);
}