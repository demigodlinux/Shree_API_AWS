using System;
using System.Collections.Generic;

namespace Shree_API_AWS.Models;

public partial class TimesheetdetailsAdmin
{
    public int Id { get; set; }

    public string Employeeid { get; set; } = null!;

    public DateTime Dataenteredon { get; set; }

    public string? Dataenteredby { get; set; }

    public int? Timesheetdetailid { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual TimesheetdetailsEmployee? Timesheetdetail { get; set; }
}
