namespace VerifyTests.Wolverine;

public delegate IReadOnlyList<Envelope> PreviewSubscription(object message, DeliveryOptions options);

public partial class RecordingMessageContext
{
    PreviewSubscription previewSubscription;

    public IReadOnlyList<Envelope> PreviewSubscriptions(object message) =>
        previewSubscription.Invoke(message, new());

    public IReadOnlyList<Envelope> PreviewSubscriptions(object message, DeliveryOptions options) =>
        previewSubscription.Invoke(message, options);
}