namespace FxferService.Infrastructure.Persistence.Batch.Contracts;

public sealed class SourceScheduleModel
{
    public List<DayOfWeek> DaysToRun { get; set; } = new();

    public List<int> MonthlyProcessDayToRun { get; set; } = new();

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }
}
