using System;
using System.Collections.Generic;

namespace Shree_API_AWS.Models;

public partial class Employeeattendancetype
{
    public string Employeeid { get; set; } = null!;

    public string? Entryformonth { get; set; }

    public DateTime? Dataenteredon { get; set; }

    public string? Dataenteredby { get; set; }

    public bool? Ispresent { get; set; }

    public DateTime? Lastpresentdate { get; set; }

    public int? Totaldayspresent { get; set; }

    public bool? Isabsent { get; set; }

    public DateTime? Lastabsentdate { get; set; }

    public int? Totaldaysabsent { get; set; }

    public bool? Ispaidleave { get; set; }

    public DateTime? Lastpaidleavedate { get; set; }

    public bool? Islate { get; set; }

    public DateTime? Lastlateday { get; set; }

    public int? Totaldayslateday { get; set; }

    public bool? Ishalfday { get; set; }

    public DateTime? Lasthalfdaydate { get; set; }

    public int? Totaldayshalfdays { get; set; }
}
