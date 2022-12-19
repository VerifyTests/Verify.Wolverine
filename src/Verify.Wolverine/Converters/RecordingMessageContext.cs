namespace VerifyTests.Wolverine;

public partial class RecordingMessageContext :
    IMessageContext
{
    public RecordingMessageContext(object? message = null, PreviewSubscription? previewSubscription = null)
    {
        this.previewSubscription = previewSubscription ?? (_ => new List<Envelope>());

        if (message != null)
        {
            Envelope = new(message);
        }

        CorrelationId = Guid.NewGuid().ToString();
    }

    public string? CorrelationId { get; set; }
    public Envelope? Envelope { get; }
}
