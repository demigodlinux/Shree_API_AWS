using System;
using System.Collections.Generic;

namespace Shree_API_AWS.Models;

public partial class Userdetailstable
{
    public int Id { get; set; }

    public string Employeeid { get; set; } = null!;

    public DateTime? Createddate { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? Priority { get; set; }

    public bool? Isactive { get; set; }

    public string? Role { get; set; }

    public string? Email { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
