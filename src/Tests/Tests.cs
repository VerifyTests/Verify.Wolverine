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
        var messageContext = new RecordingMessageContext();
        var handler = new Handler(messageContext);
        handler.Handle(new Message());
        return Verify(messageContext);
    }

    #endregion
}

public record Message();

public class Handler
{
    IMessageContext context;

    public Handler(IMessageContext context) =>
        this.context = context;

    public void Handle(Message message) =>
        context.SendAsync(new Response());
}

public record Response;

