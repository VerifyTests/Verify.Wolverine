using VerifyTests.Wolverine;
using Wolverine;
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

[UsesVerify]
public class Tests
{
    #region HandlerTest

    [Fact]
    public Task HandlerTest()
    {
        var context = new RecordingMessageContext();
        var handler = new Handler(context);
        handler.Handle(new Message());
        return Verify(context);
    }

    #endregion
}

#region Handler
public record Message;

public record Response(string Property);

public class Handler
{
    IMessageContext context;

    public Handler(IMessageContext context) =>
        this.context = context;

    public void Handle(Message message) =>
        context.SendAsync(new Response("Property Value"));
}

#endregion

