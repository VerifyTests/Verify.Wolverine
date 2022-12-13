namespace VerifyTests.Wolverine;

public partial class RecordingMessageContext
{
    List<Broadcasted> broardcasted = new();
    public IReadOnlyList<Broadcasted> Broardcasted => broardcasted;

    public ValueTask BroadcastToTopicAsync(string topic, object message, DeliveryOptions? options = null)
    {
        broardcasted.Add(new(topic, message, options));
        return ValueTask.CompletedTask;
    }
}

public record Broadcasted(string Topic, object Message, DeliveryOptions? Options = null);