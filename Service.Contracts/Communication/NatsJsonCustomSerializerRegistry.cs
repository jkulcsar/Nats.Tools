using NATS.Client.Core;
using NATS.Client.Serializers.Json;

namespace Service.Contracts.Communication;

public sealed class NatsJsonCustomSerializerRegistry : INatsSerializerRegistry
{
    public static readonly NatsJsonCustomSerializerRegistry Default = new();

    public INatsSerialize<T> GetSerializer<T>() => new NatsJsonSerializer<T>(DefaultJsonSerializerOptions.Default);

    public INatsDeserialize<T> GetDeserializer<T>() => new NatsJsonSerializer<T>(DefaultJsonSerializerOptions.Default);
}