namespace VerifyTests.Wolverine;

public delegate T? InvokeResult<out T>(object message);

public partial class RecordingMessageContext
{
    List<Invoked> invoked = new();

    public IReadOnlyList<Invoked> Invoked => invoked;

    public Task InvokeAsync(object message, CancellationToken cancellation = default, TimeSpan? timeout = null)
    {
        AddInvoke(message, timeout);
        return Task.CompletedTask;
    }

    internal void AddInvoke(object message, TimeSpan? timeout, string? endpoint = null) =>
        invoked.Add(new(message, timeout, endpoint));

    Dictionary<Type, Func<object, object?>> invokeResults = new();

    public void SetInvokeResult<T>(T? result) =>
        SetInvokeResult(_ => result);

    public void SetInvokeResult<T>(InvokeResult<T> invokeResult) =>
        invokeResults[typeof(T)] = _ => invokeResult(_);

    public Task<T?> InvokeAsync<T>(object message, CancellationToken cancellation = default, TimeSpan? timeout = null) =>
        AddInvoke<T>(message, timeout);

    internal Task<T?> AddInvoke<T>(object message, TimeSpan? timeout = null, string? endpoint = null)
    {
        invoked.Add(new(message, timeout, endpoint));

        if (invokeResults.TryGetValue(typeof(T), out var func))
        {
            return Task.FromResult((T?)func(message));
        }

        return Task.FromResult<T?>(default);
    }
}

public record Invoked(object Message, TimeSpan? Timeout = null, string? Endpoint = null);