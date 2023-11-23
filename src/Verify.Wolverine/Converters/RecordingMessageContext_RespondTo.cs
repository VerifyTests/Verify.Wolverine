namespace VerifyTests.Wolverine;

public partial class RecordingMessageContext
{
    List<object> responses = [];
    public IReadOnlyList<object> Responses => responses;

    public ValueTask RespondToSenderAsync(object response)
    {
        responses.Add(response);
        return ValueTask.CompletedTask;
    }
}