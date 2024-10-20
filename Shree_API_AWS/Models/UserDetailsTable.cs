using System;
using System.Collections.Generic;

namespace Shree_API_AWS.Models;

public partial class UserDetailsTable
{
    public int Id { get; set; }

    public string EmployeeId { get; set; } = null!;

    public DateTime? CreatedDate { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? Priority { get; set; }

    public bool? IsActive { get; set; }

    public string? Role { get; set; }

    public string? Email { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
