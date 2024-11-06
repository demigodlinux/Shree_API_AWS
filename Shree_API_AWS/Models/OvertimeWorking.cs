using System;
using System.Collections.Generic;

namespace Shree_API_AWS.Models;

public partial class Overtimeworking
{
    public int Id { get; set; }

    public string Employeeid { get; set; } = null!;

    public string? Entryformonth { get; set; }

    public DateTime? Dataenteredon { get; set; }

    public string? Dataenteredby { get; set; }

    public bool? Issundayduty { get; set; }

    public DateTime? Lastsundayduty { get; set; }

    public int? Totalsundayduty { get; set; }

    public bool? Ispublicholidayduty { get; set; }

    public DateTime? Lastpublicholidayduty { get; set; }

    public int? Totalpublicholidayduty { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
