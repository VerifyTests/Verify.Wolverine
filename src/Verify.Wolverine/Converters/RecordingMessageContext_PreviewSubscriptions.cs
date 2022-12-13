namespace VerifyTests.Wolverine;

public delegate IReadOnlyList<Envelope> PreviewSubscription(object message);

public partial class RecordingMessageContext
{
    PreviewSubscription previewSubscription;

    public IReadOnlyList<Envelope> PreviewSubscriptions(object message) =>
        previewSubscription.Invoke(message);
}