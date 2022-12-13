using System.Diagnostics.CodeAnalysis;

namespace VerifyTests.Wolverine;

public partial class RecordingMessageContext :
    IMessageContext
{
    static AsyncLocal<RecordingMessageContext?> local = new();

    public static bool TryFinishRecording([NotNullWhen(true)] out RecordingMessageContext? context)
    {
        context = local.Value;
        return context != null;
    }

    public RecordingMessageContext(object? message = null, PreviewSubscription? previewSubscription = null)
    {
        local.Value = this;

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
