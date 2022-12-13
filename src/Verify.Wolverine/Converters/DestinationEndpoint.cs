namespace VerifyTests.Wolverine;

public class DestinationEndpoint : IDestinationEndpoint
{
    RecordingMessageContext context;

    public DestinationEndpoint(RecordingMessageContext context, string endpoint)
    {
        this.context = context;
        EndpointName = endpoint;
    }

    public ValueTask SendAsync<T>(T message, DeliveryOptions? options = null)
    {
        context.AddSend(message, options, EndpointName);
        return ValueTask.CompletedTask;
    }

    public Task<Acknowledgement> InvokeAsync(object message, CancellationToken cancellation = default, TimeSpan? timeout = null)
    {
        context.AddInvoke(message, timeout, EndpointName);
        return Task.FromResult(new Acknowledgement());
    }

    public async Task<T> InvokeAsync<T>(object message, CancellationToken cancellation = default, TimeSpan? timeout = null)
        where T : class
    {
        var invoke = await context.AddInvoke<T>(message, timeout, EndpointName);
        return invoke!;
    }

    public Uri Uri => new(EndpointName);
    public string EndpointName { get; }
}