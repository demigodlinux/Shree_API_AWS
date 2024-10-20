using System;
using System.Collections.Generic;

namespace Shree_API_AWS.Models;

public partial class LogEmployeeAttendance
{
    public int Id { get; set; }

    public string EmployeeId { get; set; } = null!;

    public int? EmployeeAttendId { get; set; }

    public string? EntryForMonth { get; set; }

    public DateTime? DataEnteredOn { get; set; }

    public string? DataEnteredBy { get; set; }

    public DateTime? AttendanceDate { get; set; }

    public bool? IsPresent { get; set; }

    public bool? IsAbsent { get; set; }

    public bool? IsPaidLeave { get; set; }

    public bool? IsLate { get; set; }

    public bool? IsHalfDay { get; set; }

    public bool? IsOnDuty { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual EmployeeAttendance? EmployeeAttend { get; set; }
}
