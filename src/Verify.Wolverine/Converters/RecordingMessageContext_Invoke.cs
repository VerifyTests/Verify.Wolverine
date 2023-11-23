namespace VerifyTests.Wolverine;

public delegate T InvokeResult<out T>(object message)
    where T: notnull;

public partial class RecordingMessageContext
{
    List<Invoked> invoked = [];

    public IReadOnlyList<Invoked> Invoked => invoked;

    public Task InvokeAsync(object message, Cancel cancel = default, TimeSpan? timeout = null)
    {
        AddInvoke(message, timeout);
        return Task.CompletedTask;
    }

    internal void AddInvoke(object message, TimeSpan? timeout, string? endpoint = null) =>
        invoked.Add(new(message, timeout, endpoint));

    Dictionary<Type, Func<object, object>> invokeResults = [];

    public void AddInvokeResult<T>(T result) where T : notnull =>
        AddInvokeResult(_ => result);

    public void AddInvokeResult<T>(InvokeResult<T> invokeResult)
        where T : notnull =>
        invokeResults[typeof(T)] = _ => invokeResult(_);

    public Task<T> InvokeAsync<T>(object message, Cancel cancel = default, TimeSpan? timeout = null) =>
        AddInvoke<T>(message, timeout);

    public Task InvokeForTenantAsync(string tenantId, object message, Cancel cancel = default, TimeSpan? timeout = null)
    {
        invoked.Add(new(message, timeout, null, tenantId));
        return Task.CompletedTask;
    }

    public Task<T> InvokeForTenantAsync<T>(string tenantId, object message, Cancel cancel = default, TimeSpan? timeout = null) =>
        AddInvoke<T>(message, timeout,null,tenantId);

    internal Task<T> AddInvoke<T>(object message, TimeSpan? timeout = null, string? endpoint = null, string? tenant = null)
    {
        invoked.Add(new(message, timeout, endpoint, tenant));

        var type = typeof(T);
        if (invokeResults.TryGetValue(type, out var func))
        {
            return Task.FromResult((T) func(message));
        }

        if (type.IsValueType)
        {
            return Task.FromResult((T)default!);
        }

        var constructor = type.GetConstructor(Type.EmptyTypes);
        if (constructor is not null)
        {
            return Task.FromResult((T) constructor.Invoke(null));
        }

        throw new($"Not SetInvokeResult has been defined for {type}");
    }
}

public record Invoked(object Message, TimeSpan? Timeout = null, string? Endpoint = null, string? Tenant = null);