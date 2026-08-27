namespace VerifyTests.Wolverine;

public partial class RecordingMessageContext
{
    List<Streamed> streamed = [];
    public IReadOnlyList<Streamed> Streamed => streamed;

    List<StreamedRequest> streamedRequests = [];
    public IReadOnlyList<StreamedRequest> StreamedRequests => streamedRequests;

    Dictionary<Type, Func<object, IEnumerable<object>>> streamResults = [];

    Dictionary<Type, Func<object, object>> streamRequestResults = [];

    public void AddStreamResult<T>(params T[] results)
        where T : notnull =>
        streamResults[typeof(T)] = _ => results.Cast<object>();

    public void AddStreamRequestResult<T>(T result)
        where T : notnull =>
        streamRequestResults[typeof(T)] = _ => result;

    public IAsyncEnumerable<TResponse> StreamAsync<TResponse>(object message, Cancel cancellation = default)
    {
        streamed.Add(new(message));
        return Stream<TResponse>(message);
    }

    public IAsyncEnumerable<TResponse> StreamAsync<TResponse>(object message, DeliveryOptions options, Cancel cancellation = default)
    {
        streamed.Add(new(message, options));
        return Stream<TResponse>(message);
    }

    public async Task<TResponse> StreamAsync<TRequest, TResponse>(IAsyncEnumerable<TRequest> messages, Cancel cancellation = default, TimeSpan? timeout = null)
    {
        var list = await ToList(messages, cancellation);
        streamedRequests.Add(new(list, null, timeout));
        return BuildResult<TResponse>(streamRequestResults, list, nameof(AddStreamRequestResult));
    }

    public async Task<TResponse> StreamAsync<TRequest, TResponse>(IAsyncEnumerable<TRequest> messages, DeliveryOptions options, Cancel cancellation = default, TimeSpan? timeout = null)
    {
        var list = await ToList(messages, cancellation);
        streamedRequests.Add(new(list, options, timeout));
        return BuildResult<TResponse>(streamRequestResults, list, nameof(AddStreamRequestResult));
    }

    static async Task<IReadOnlyList<object>> ToList<TRequest>(IAsyncEnumerable<TRequest> messages, Cancel cancellation)
    {
        var list = new List<object>();
        await foreach (var message in messages.WithCancellation(cancellation))
        {
            list.Add(message!);
        }

        return list;
    }

    async IAsyncEnumerable<TResponse> Stream<TResponse>(object message)
    {
        await Task.CompletedTask;

        if (!streamResults.TryGetValue(typeof(TResponse), out var func))
        {
            yield break;
        }

        foreach (var item in func(message))
        {
            yield return (TResponse) item;
        }
    }
}

public record Streamed(object Message, DeliveryOptions? Options = null);

public record StreamedRequest(IReadOnlyList<object> Messages, DeliveryOptions? Options = null, TimeSpan? Timeout = null);
