namespace VerifyTests.Wolverine;

public partial class RecordingMessageContext :
    IMessageContext
{
    public RecordingMessageContext(object? message = null, PreviewSubscription? previewSubscription = null)
    {
        this.previewSubscription = previewSubscription ?? ((_, _) => []);

        if (message != null)
        {
            Envelope = new(message);
        }

        CorrelationId = Guid.NewGuid().ToString();
    }

    [Argon.JsonIgnore]
    public string? CorrelationId { get; set; }
    public Envelope? Envelope { get; }
    public string? TenantId { get; set; }
}
