namespace VerifyTests.Wolverine;

public partial class RecordingMessageContext
{
    List<Sent> sent = [];
    public IReadOnlyList<Sent> Sent => sent;

    public ValueTask SendAsync<T>(T message, DeliveryOptions? options = null)
    {
        AddSend(message, options);
        return ValueTask.CompletedTask;
    }

    internal void AddSend<T>(T message, DeliveryOptions? options, string? endpoint = null) =>
        sent.Add(new(message!, options, endpoint));
}

public record Sent(object Message, DeliveryOptions? Options = null, string? Endpoint = null);