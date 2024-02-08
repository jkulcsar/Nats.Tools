using Microsoft.Extensions.Logging;
using NATS.Client.Core;
using Service.Contracts.Features.Weather;

namespace CorePublishSubscribe.Subscriber;

class SimpleSubscriberService(INatsConnection natsConnection, ILogger<SimpleSubscriberService> logger) : ISubscriberService
{
    public async Task<Task> ListenToSubject(string subject, CancellationToken cancellationToken)
    {
        await using var sub = await natsConnection.SubscribeCoreAsync<WeatherResponse>(subject, cancellationToken: cancellationToken);
        
        logger.LogInformation("Waiting for messages...");
        
        var task = Task.Run(async () =>
        {
            await foreach (var msg in sub.Msgs.ReadAllAsync(cancellationToken))
            {
                var data = msg.Data;
                logger.LogInformation("Subscriber received {Subject}: {Data}", msg.Subject, data);
            }

            logger.LogInformation("Unsubscribed");
        });

        return task;
    }
}