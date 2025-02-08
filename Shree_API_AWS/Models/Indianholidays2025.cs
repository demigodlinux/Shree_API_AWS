using System;
using System.Collections.Generic;

namespace Shree_API_AWS.Models;

public partial class Indianholidays2025
{
    public int Holidayid { get; set; }

    public DateOnly Holidaydate { get; set; }

    public string Holidayname { get; set; } = null!;

    public string Holidaytype { get; set; } = null!;

    public string? Region { get; set; }

    public string? Description { get; set; }

    public bool? Isactive { get; set; }
}
