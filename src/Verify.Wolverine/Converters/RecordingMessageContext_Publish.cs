namespace VerifyTests.Wolverine;

public partial class RecordingMessageContext
{
    List<Published> published = [];
    public IReadOnlyList<Published> Published => published;

    public ValueTask PublishAsync<T>(T message, DeliveryOptions? options = null)
    {
        published.Add(new(message!, options));
        return ValueTask.CompletedTask;
    }
}

public record Published(object Message, DeliveryOptions? Timeout = null);