using System.Text.Json;
using Cocona;
using CorePublishSubscribe.Publisher;
using CorePublishSubscribe.Subscriber;
using Microsoft.Extensions.Logging;

namespace CorePublishSubscribe;


public class RunCommands(IPublisherService publisherService, ISubscriberService subscriberService, ILogger<RunCommands> logger)
{
    [Command("scenario")]
    public async Task Scenario(int count)
    {
        await subscriberService.ListenToSubject("weather.>", default);
        var result =  publisherService.PublishRandomMessages(count);
        
        // await foreach (var weather in result)
        // {
        //     logger.LogInformation(JsonSerializer.Serialize(weather, new JsonSerializerOptions{WriteIndented = true}));
        // }
    }
  
}

