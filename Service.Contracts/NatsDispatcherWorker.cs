using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using NATS.Client.Serializers.Json;
using Service.Contracts.Communication;

namespace Service.Contracts;

public class NatsDispatcherWorker(INatsConnection nats, IMediator mediator, ILogger<NatsDispatcherWorker> logger)
    : BackgroundService
{
    const string WeatherStreamName = "weather-stream";
    private static readonly string[] SubjectsOfWeatherStream = ["weather.>"];

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var parallelOptions = new ParallelOptions
        {
            CancellationToken = stoppingToken,
            MaxDegreeOfParallelism = Math.Max(2, Environment.ProcessorCount - 2)
        };
        
        await EnsureStreamAsync(stoppingToken);

        var messages =
            nats.SubscribeAsync(
                subject: ">", 
                serializer: new NatsJsonSerializer<MessageEnvelope?>(DefaultJsonSerializerOptions.Default),
                cancellationToken: stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Parallel.ForEachAsync(
                    messages,
                    parallelOptions,
                    async (message, cancellationToken) =>
                    {
                        var data = message.Data?.GetData();
                        var dataTypeName = data?.GetType().Name ?? "null";

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
                                    //replyTo: replyRequest.ReplyTo,
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

    private async Task EnsureStreamAsync(CancellationToken stoppingToken)
    {
        var js = new NatsJSContext((NatsConnection)nats);

        var streamConfig = new StreamConfig
        (
            name: WeatherStreamName,
            subjects: SubjectsOfWeatherStream
        )
        {
            Storage = StreamConfigStorage.Memory
        };


        var existingStreams = js.ListStreamNamesAsync(cancellationToken: stoppingToken);
        await foreach (var streamName in existingStreams)
        {
            logger.LogInformation("Existing JetStream stream: {StreamName}", streamName);
            if(WeatherStreamName.Equals(streamName))
                await js.DeleteStreamAsync(WeatherStreamName, stoppingToken);
        }
        
        logger.LogInformation("Creating JetStream stream {StreamName}", streamConfig.Name);
        var stream = await js.CreateStreamAsync(streamConfig, stoppingToken);
        await PrintStreamStateAsync(stream);
        logger.LogInformation("JetStream stream {StreamName} created", streamConfig.Name);
    }
    
    async Task PrintStreamStateAsync(INatsJSStream jsStream)
    {
        await jsStream.RefreshAsync();
        var state = jsStream.Info.State;
        logger.LogInformation("Stream has {Messages} messages using {Bytes} bytes", state.Messages, state.Bytes);
    }
}