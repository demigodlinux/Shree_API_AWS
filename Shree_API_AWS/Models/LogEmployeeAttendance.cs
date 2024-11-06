using System;
using System.Collections.Generic;

namespace Shree_API_AWS.Models;

public partial class LogEmployeeattendance
{
    public int Id { get; set; }

    public string Employeeid { get; set; } = null!;

    public int? Employeeattendid { get; set; }

    public string? Entryformonth { get; set; }

    public DateTime? Dataenteredon { get; set; }

    public string? Dataenteredby { get; set; }

    public DateTime? Attendancedate { get; set; }

    public bool? Ispresent { get; set; }

    public bool? Isabsent { get; set; }

    public bool? Ispaidleave { get; set; }

    public bool? Islate { get; set; }

    public bool? Ishalfday { get; set; }

    public bool? Isonduty { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Employeeattendance? Employeeattend { get; set; }
}
