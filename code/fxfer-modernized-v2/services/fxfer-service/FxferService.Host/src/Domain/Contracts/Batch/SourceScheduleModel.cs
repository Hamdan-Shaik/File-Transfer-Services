namespace FxferService.Domain.Contracts.Batch;

public class SourceScheduleModel
{
    public List<DayOfWeek>? DaysToRun { get; set; }

    public List<int>? MonthlyProcessDayToRun { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }
}

