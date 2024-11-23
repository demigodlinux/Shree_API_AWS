using System;
using System.Collections.Generic;

namespace Shree_API_AWS.Models;

public partial class Blobtable
{
    public int Id { get; set; }

    public string Employeeid { get; set; } = null!;

    public string? Sitename { get; set; }

    public string? Sitelocation { get; set; }

    public string? Servicetype { get; set; }

    public string? Typeofupload { get; set; }

    public string? Filename { get; set; }

    public string? Filepath { get; set; }

    public DateTime? Dateenteredon { get; set; }

    public string? Dateenteredby { get; set; }
}
