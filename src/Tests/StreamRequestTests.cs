// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

public class StreamRequestTests
{
    [Fact]
    public async Task StreamRequest()
    {
        var context = new RecordingMessageContext();
        context.AddStreamRequestResult(new Response("Response"));
        var handler = new Handler(context);
        var response = await handler.Handle(new Message("value"));
        await Verify(
            new
            {
                context,
                response
            });
    }

    class Handler(IMessageBus context)
    {
        public Task<Response> Handle(Message message) =>
            context.StreamAsync<Request, Response>(Requests(message));

        static async IAsyncEnumerable<Request> Requests(Message message)
        {
            await Task.CompletedTask;
            yield return new(message.Property);
            yield return new($"{message.Property} two");
        }
    }
}
