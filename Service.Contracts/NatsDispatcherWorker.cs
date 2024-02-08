using CloudNative.CloudEvents;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NATS.Client.Core;
using NATS.Client.Serializers.Json;
using Service.Contracts.Communication;

namespace Service.Contracts;

public class NatsDispatcherWorker(INatsConnection nats, IMediator mediator, ILogger<NatsDispatcherWorker> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var parallelOptions = new ParallelOptions
        {
            CancellationToken = stoppingToken,
            MaxDegreeOfParallelism = Math.Max(2, Environment.ProcessorCount - 2)
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var messages =
                    nats.SubscribeAsync<MessageEnvelope>(
                        subject: ">", 
                        serializer: new NatsJsonSerializer<MessageEnvelope>(DefaultJsonSerializerOptions.Default),
                        cancellationToken: stoppingToken);

                await Parallel.ForEachAsync(
                    messages,
                    parallelOptions,
                    async (message, cancellationToken) =>
                    {
                        var data = message.Data?.GetData();

                        switch (data)
                        {
                            case INotification notification:
                                await mediator.Publish(notification, cancellationToken);
                                break;
                            case IRequest request:
                                await mediator.Send(request, cancellationToken);
                                await message.ReplyAsync(cancellationToken: cancellationToken);
                                break;
                            case IBaseRequest replyRequest:
                                var result = await mediator.Send(replyRequest, cancellationToken);
                                await message.ReplyAsync(
                                    data: result, 
                                    cancellationToken: cancellationToken);
                                break;
                            default:
                                throw new NotSupportedException("Something went wrong");
                        }
                    });
            }
            catch (Exception e)
            {
                logger.LogError(e, "NATS subscription failed, retrying");
                await Task.Delay(100, stoppingToken);
            }
        }
    }
}