using System;
using System.Collections.Generic;

namespace Shree_API_AWS.Models;

public partial class TimesheetdetailsEmployee
{
    public int Id { get; set; }

    public string? Employeeid { get; set; }

    public DateTime? Dataenteredon { get; set; }

    public string? Dataenteredby { get; set; }

    public string? Timesheetdetails { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<TimesheetdetailsAdmin> TimesheetdetailsAdmins { get; set; } = new List<TimesheetdetailsAdmin>();
}
