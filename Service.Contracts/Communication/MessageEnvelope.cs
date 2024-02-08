using System.Text.Json;
using MediatR;

namespace Service.Contracts.Communication;

public record MessageEnvelope()
{
    public required string SerializedData { get; init; }
    public required string DataType { get; init; }

    public static MessageEnvelope CreateRequestMessage<T>(T data) where T : IBaseRequest =>
        CreateMessageInternal(data);

    public static MessageEnvelope CreateNotificationMessage<T>(T data) where T : INotification =>
        CreateMessageInternal(data);

    private static MessageEnvelope CreateMessageInternal<T>(T data) => new()
    {
        SerializedData = JsonSerializer.Serialize(data, DefaultJsonSerializerOptions.Default),
        DataType = typeof(T).FullName ?? throw new NullReferenceException()
    };

    public object GetData()
    {
        var type = Type.GetType(DataType) ?? throw new NotSupportedException("Type not found");
        var data = JsonSerializer.Deserialize(SerializedData, type, DefaultJsonSerializerOptions.Default) ??
                   throw new NotSupportedException("Could not deserialize message");
        if (data is not IBaseRequest or INotification)
        {
            throw new NotSupportedException("Not Mediatr format");
        }

        return data;
    }
}