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
    public string? UserName { get; set; }

    static T BuildResult<T>(Dictionary<Type, Func<object, object>> results, object message, string addMethod)
    {
        var type = typeof(T);
        if (results.TryGetValue(type, out var func))
        {
            return (T) func(message);
        }

        if (type.IsValueType)
        {
            return default!;
        }

        var constructor = type.GetConstructor(Type.EmptyTypes);
        if (constructor is not null)
        {
            return (T) constructor.Invoke(null);
        }

        throw new($"No {addMethod} has been defined for {type}");
    }
}
