namespace VerifyTests.Wolverine;

public partial class RecordingMessageContext
{
    List<Streamed> streamed = [];
    public IReadOnlyList<Streamed> Streamed => streamed;

    Dictionary<Type, Func<object, IEnumerable<object>>> streamResults = [];

    public void AddStreamResult<T>(params T[] results)
        where T : notnull =>
        streamResults[typeof(T)] = _ => results.Cast<object>();

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
