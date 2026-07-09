// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

public class StreamResultTests
{
    [Fact]
    public async Task StreamResult()
    {
        var context = new RecordingMessageContext();
        context.AddStreamResult(
            new Response("Response one"),
            new Response("Response two"));
        var handler = new Handler(context);
        await handler.Handle(new Message("value"));
        await Verify(
            new
            {
                context,
                handler.Received
            });
    }

    class Handler(IMessageBus context)
    {
        public List<Response> Received { get; } = [];

        public async Task Handle(Message message)
        {
            var request = new Request(message.Property);
            await foreach (var response in context.StreamAsync<Response>(request))
            {
                Received.Add(response);
            }
        }
    }
}
