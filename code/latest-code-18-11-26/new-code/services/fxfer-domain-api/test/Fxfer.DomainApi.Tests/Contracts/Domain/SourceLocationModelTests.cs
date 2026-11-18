using System.Reflection;
using Fxfer.DomainApi.Contracts.Domain;

namespace Fxfer.DomainApi.Tests.Contracts.Domain;

public sealed class SourceLocationModelTests
{
    [Fact]
    public void SourceLocationModel_IsScheduledToRun_is_true_when_Schedule_is_null()
    {
        var model = new SourceLocationModel
        {
            Schedule = null,
        };

        Assert.True(model.IsScheduledToRun);
    }

    [Fact]
    public void SourceLocationModel_IsScheduledToRun_is_false_when_now_is_outside_time_window()
    {
        var now = DateTime.Now;

        var model = new SourceLocationModel
        {
            Schedule = new SourceScheduleModel
            {
                StartTime = TimeSpan.FromHours(23),
                EndTime = TimeSpan.FromHours(1),
                DaysToRun = new List<DayOfWeek> { now.DayOfWeek },
                MonthlyProcessDayToRun = new List<int> { now.Day },
            },
        };

        Assert.False(model.IsScheduledToRun);
    }

    [Fact]
    public void SourceLocationModel_IsScheduledToRun_is_true_when_now_is_in_window_and_today_is_in_DaysToRun()
    {
        var now = DateTime.Now;

        var model = new SourceLocationModel
        {
            Schedule = new SourceScheduleModel
            {
                StartTime = TimeSpan.Zero,
                EndTime = TimeSpan.FromDays(1) - TimeSpan.FromTicks(1),
                DaysToRun = new List<DayOfWeek> { now.DayOfWeek },
                MonthlyProcessDayToRun = new List<int>(),
            },
        };

        Assert.True(model.IsScheduledToRun);
    }

    [Fact]
    public void SourceLocationModel_IsScheduledToRun_is_true_when_now_is_in_window_and_today_is_in_MonthlyProcessDayToRun()
    {
        var now = DateTime.Now;

        var model = new SourceLocationModel
        {
            Schedule = new SourceScheduleModel
            {
                StartTime = TimeSpan.Zero,
                EndTime = TimeSpan.FromDays(1) - TimeSpan.FromTicks(1),
                DaysToRun = new List<DayOfWeek>(),
                MonthlyProcessDayToRun = new List<int> { now.Day },
            },
        };

        Assert.True(model.IsScheduledToRun);
    }

    [Fact]
    public void SourceLocationModel_IsScheduledToRun_is_false_when_now_is_in_window_but_no_day_matches()
    {
        var now = DateTime.Now;
        var otherDayOfWeek = (DayOfWeek)(((int)now.DayOfWeek + 1) % 7);
        var otherDayOfMonth = now.Day == 1 ? 2 : 1;

        var model = new SourceLocationModel
        {
            Schedule = new SourceScheduleModel
            {
                StartTime = TimeSpan.Zero,
                EndTime = TimeSpan.FromDays(1) - TimeSpan.FromTicks(1),
                DaysToRun = new List<DayOfWeek> { otherDayOfWeek },
                MonthlyProcessDayToRun = new List<int> { otherDayOfMonth },
            },
        };

        Assert.False(model.IsScheduledToRun);
    }

    [Fact]
    public void SourceLocationModel_IsTimeOfDayBetween_is_inclusive()
    {
        var now = DateTime.Now;
        var model = new SourceLocationModel();

        var method = typeof(SourceLocationModel).GetMethod(
            "IsTimeOfDayBetween",
            BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.NotNull(method);

        var result = (bool)method!.Invoke(model, new object[] { now, now.TimeOfDay, now.TimeOfDay })!;

        Assert.True(result);
    }

    [Fact]
    public void SourceLocationModel_constructor_initializes_Destinations_to_empty_list()
    {
        var model = new SourceLocationModel();

        Assert.NotNull(model.Destinations);
        Assert.Empty(model.Destinations);
    }
}