using VerifyTests.Wolverine;
using Wolverine;
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

[UsesVerify]
public class Tests
{
    #region HandlerTest

    [Fact]
    public async Task HandlerTest()
    {
        var context = new RecordingMessageContext();
        var handler = new Handler(context);
        await handler.Handle(new Message("value"));
        await Verify(context);
    }

    #endregion

    [Fact]
    public async Task AllTest()
    {
        var context = new RecordingMessageContext();
        var handler = new AllHandler(context);
        await handler.Handle(new Message("value"));
        await Verify(context);
    }
}

#region Handler

public class Handler
{
    IMessageContext context;

    public Handler(IMessageContext context) =>
        this.context = context;

    public ValueTask Handle(Message message) =>
        context.SendAsync(new Response("Property Value"));
}

#endregion

public class AllHandler
{
    IMessageContext context;

    public AllHandler(IMessageContext context) =>
        this.context = context;

    public async ValueTask Handle(Message message)
    {
        await context.SendAsync(
            new Response("Property Value"),
            new DeliveryOptions
            {
                DeliverWithin = TimeSpan.FromDays(1)
            });
        await context.RespondToSenderAsync(
            new Response("Property Value"));
        await context.InvokeAsync(
            new Response("Property Value"),
            timeout: TimeSpan.FromDays(2));
        await context.InvokeAsync<Guid>(
            new Response("Property Value"),
            timeout: TimeSpan.FromDays(2));
        await context.BroadcastToTopicAsync(
            "topic",
            new Response("Property Value"),
            new DeliveryOptions
            {
                DeliverWithin = TimeSpan.FromDays(3)
            });
        var destination = context.EndpointFor("endpoint");
        await destination.SendAsync(
            new Response("Property Value"),
            new DeliveryOptions
            {
                DeliverWithin = TimeSpan.FromDays(1)
            });
        await destination.InvokeAsync(
            new Response("Property Value"),
            timeout: TimeSpan.FromDays(2));
    }
}