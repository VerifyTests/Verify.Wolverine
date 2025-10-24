namespace VerifyTests.Wolverine;

public partial class RecordingMessageContext
{
    List<ReSchedule> reScheduled = [];
    public IReadOnlyList<ReSchedule> ReScheduled => reScheduled;

    public Task ReScheduleCurrentAsync(DateTimeOffset rescheduledAt)
    {
        AddReScheduleCurrent(rescheduledAt);
        return Task.CompletedTask;
    }

    internal void AddReScheduleCurrent(DateTimeOffset rescheduledAt) =>
        reScheduled.Add(new(rescheduledAt));
}

public record ReSchedule(DateTimeOffset rescheduledAt);