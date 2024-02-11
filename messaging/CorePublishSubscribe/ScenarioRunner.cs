using System.Text.Json;
using Cocona;
using CorePublishSubscribe.Publisher;
using CorePublishSubscribe.Subscriber;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CorePublishSubscribe;


public class ScenarioRunner(IPublisherService publisherService, ISubscriberService subscriberService, ILogger<ScenarioRunner> logger) : BackgroundService
{
    [Command("scenario")]
    public async Task Scenario(int count)
    {
        //await subscriberService.ListenToSubject("weather.>", default);
        //var result =  publisherService.PublishRandomMessages(count);
        
        // await foreach (var weather in result)
        // {
        //     logger.LogInformation(JsonSerializer.Serialize(weather, new JsonSerializerOptions{WriteIndented = true}));
        // }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            //await subscriberService.ListenToSubject("weather.>", default);
            var result = publisherService.PublishRandomMessages(100);
             await foreach (var weather in result)
             {
                 logger.LogInformation(JsonSerializer.Serialize(weather, new JsonSerializerOptions{WriteIndented = true}));
             }
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }
    }
}

