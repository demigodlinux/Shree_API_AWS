using System;
using System.Collections.Generic;

namespace Shree_API_AWS.Models;

public partial class Overtimeworking
{
    public int Id { get; set; }

    public string Employeeid { get; set; } = null!;

    public string Dataenteredby { get; set; } = null!;

    public DateTime Dataenteredon { get; set; }

    public string Entryfor { get; set; } = null!;

    public DateOnly Dateofotworking { get; set; }

    public float Othoursworked { get; set; }

    public bool Isactive { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
