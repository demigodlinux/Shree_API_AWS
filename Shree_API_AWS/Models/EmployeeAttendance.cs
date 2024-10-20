using System;
using System.Collections.Generic;

namespace Shree_API_AWS.Models;

public partial class EmployeeAttendance
{
    public int Id { get; set; }

    public string EmployeeId { get; set; } = null!;

    public string? EntryForMonth { get; set; }

    public DateTime? DataEnteredOn { get; set; }

    public string? DataEnteredBy { get; set; }

    public bool? IsPresent { get; set; }

    public DateTime? LastPresentDate { get; set; }

    public int? TotalDaysPresent { get; set; }

    public bool? IsAbsent { get; set; }

    public DateTime? LastAbsentDate { get; set; }

    public int? TotalDaysAbsent { get; set; }

    public bool? IsPaidLeave { get; set; }

    public DateTime? LastPaidLeaveDate { get; set; }

    public bool? IsLate { get; set; }

    public DateTime? LastLateDay { get; set; }

    public int? TotalDaysLateDay { get; set; }

    public bool? IsHalfDay { get; set; }

    public DateTime? LastHalfDayDate { get; set; }

    public int? TotalDaysHalfDays { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual ICollection<LogEmployeeAttendance> LogEmployeeAttendances { get; set; } = new List<LogEmployeeAttendance>();
}
