using System;
using System.Collections.Generic;

namespace Fxfer.ExecutionWorker.Application.Contracts.Domain;

public class SourceScheduleModel
{
    public List<DayOfWeek>? DaysToRun { get; set; }

    public List<int>? MonthlyProcessDayToRun { get; set; }

    public TimeSpan StartTime { get; set; }

    public TimeSpan EndTime { get; set; }
}

