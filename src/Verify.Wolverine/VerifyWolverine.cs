using VerifyTests.Wolverine;

namespace VerifyTests;

public static class VerifyWolverine
{
    public static void Enable()
    {
        VerifierSettings.RegisterJsonAppender(_ =>
        {
            if (!RecordingMessageContext.TryFinishRecording(out var context))
            {
                return null;
            }

            return new ToAppend(
                "messageBus",
                new
                {
                    context.Invoked,
                    context.Sent,
                    context.Responses,
                    context.Published,
                    context.Broardcasted,
                });
        });
    }
}