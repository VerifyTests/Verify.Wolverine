using VerifyTests.Wolverine;
using Wolverine;
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

public class InvokeDelegateUsage
{
    #region InvokeDelegateTest

    [Fact]
    public async Task HandlerTest()
    {
        var context = new RecordingMessageContext();
        context.AddInvokeResult<Response>(
            message =>
            {
                var request = (Request) message;
                return new Response(request.Property);
            });
        var handler = new Handler(context);
        await handler.Handle(new Message("value"));
        await Verify(context);
    }

    #endregion

    #region InvokeAsyncHandler

    public class Handler
    {
        IMessageContext context;

        public Handler(IMessageContext context) =>
            this.context = context;

        public async Task Handle(Message message)
        {
            var request = new Request(message.Property);
            var response = await context.InvokeAsync<Response>(request);
            Trace.WriteLine(response.Property);
        }
    }

    #endregion
}