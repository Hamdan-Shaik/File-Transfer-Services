using Fxfer.TransferWorker.Contracts.Batch;

namespace Fxfer.TransferWorker.Tests;

public sealed class SourceLocationModelScheduleTests
{
    [Fact]
    public void IsScheduledToRun_returns_true_when_Schedule_is_null()
    {
        var model = new SourceLocationModel
        {
            Schedule = null,
        };

        Assert.True(model.IsScheduledToRun);
    }

    [Fact]
    public void IsScheduledToRun_returns_true_when_DaysToRun_contains_today_and_time_is_in_range()
    {
        var model = new SourceLocationModel
        {
            Schedule = new SourceScheduleModel
            {
                StartTime = TimeSpan.Zero,
                EndTime = TimeSpan.FromDays(1),
                DaysToRun = new List<DayOfWeek> { DateTime.Now.DayOfWeek },
                MonthlyProcessDayToRun = new List<int>(),
            },
        };

        Assert.True(model.IsScheduledToRun);
    }

    [Fact]
    public void IsScheduledToRun_returns_true_when_MonthlyProcessDayToRun_contains_today_and_time_is_in_range()
    {
        var model = new SourceLocationModel
        {
            Schedule = new SourceScheduleModel
            {
                StartTime = TimeSpan.Zero,
                EndTime = TimeSpan.FromDays(1),
                DaysToRun = new List<DayOfWeek>(),
                MonthlyProcessDayToRun = new List<int> { DateTime.Now.Day },
            },
        };

        Assert.True(model.IsScheduledToRun);
    }

    [Fact]
    public void IsScheduledToRun_returns_false_when_time_is_outside_window_even_when_day_matches()
    {
        var nowTimeOfDayPlusOneMinute = DateTime.Now.TimeOfDay.Add(TimeSpan.FromMinutes(1));

        var model = new SourceLocationModel
        {
            Schedule = new SourceScheduleModel
            {
                StartTime = nowTimeOfDayPlusOneMinute,
                EndTime = nowTimeOfDayPlusOneMinute.Add(TimeSpan.FromMinutes(1)),
                DaysToRun = new List<DayOfWeek> { DateTime.Now.DayOfWeek },
                MonthlyProcessDayToRun = new List<int>(),
            },
        };

        Assert.False(model.IsScheduledToRun);
    }

    [Fact]
    public void IsScheduledToRun_throws_NullReferenceException_when_Schedule_lists_are_null()
    {
        var model = new SourceLocationModel
        {
            Schedule = new SourceScheduleModel
            {
                StartTime = TimeSpan.Zero,
                EndTime = TimeSpan.FromDays(1),
                DaysToRun = null,
                MonthlyProcessDayToRun = null,
            },
        };

        Assert.Throws<NullReferenceException>(() => _ = model.IsScheduledToRun);
    }
}
