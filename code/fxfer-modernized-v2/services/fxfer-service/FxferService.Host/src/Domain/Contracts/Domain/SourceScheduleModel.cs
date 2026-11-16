using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FxferService.Domain.Contracts.Domain;

/// <summary>
/// A schedule used to evaluate files
/// </summary>
[DataContract]
public class SourceScheduleModel
{
    [DataMember]
    public List<DayOfWeek>? DaysToRun { get; set; }

    [DataMember]
    public List<int>? MonthlyProcessDayToRun { get; set; }

    [DataMember]
    public TimeSpan StartTime { get; set; }

    [DataMember]
    public TimeSpan EndTime { get; set; }
}

