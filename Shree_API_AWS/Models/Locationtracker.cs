using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shree_API_AWS.Models;

public partial class Locationtracker
{
    public int Id { get; set; }

    public string Employeeid { get; set; } = null!;

    [Column(TypeName = "timestamp with time zone")]
    public DateTime Dateenteredon { get; set; }

    public string? Dateenteredby { get; set; }

    public string? Locationlog { get; set; }

    public bool? Isactive { get; set; }
}
