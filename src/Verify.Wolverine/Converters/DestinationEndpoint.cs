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

    public Task<Acknowledgement> InvokeAsync(object message, Cancel cancel = default, TimeSpan? timeout = null)
    {
        context.AddInvoke(message, timeout, EndpointName);
        return Task.FromResult(new Acknowledgement());
    }

    public Task<T> InvokeAsync<T>(object message, Cancel cancel = default, TimeSpan? timeout = null)
        where T : class =>
        context.AddInvoke<T>(message, timeout, EndpointName);

    public Uri Uri => new(EndpointName);
    public string EndpointName { get; }
}