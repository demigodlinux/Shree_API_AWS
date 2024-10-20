using System;
using System.Collections.Generic;

namespace Shree_API_AWS.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string EmployeeId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? LastName { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public long? MobileNumber { get; set; }

    public string? Ecname { get; set; }

    public int? Ecnumber { get; set; }

    public string? Position { get; set; }

    public DateTime? HireDate { get; set; }

    public decimal? Salary { get; set; }

    public decimal? Basic { get; set; }

    public decimal? Hra { get; set; }

    public decimal? SpecialAllowance { get; set; }

    public string? NativePlace { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<EmployeeAttendance> EmployeeAttendances { get; set; } = new List<EmployeeAttendance>();

    public virtual ICollection<EmployeeLoanDetail> EmployeeLoanDetails { get; set; } = new List<EmployeeLoanDetail>();

    public virtual ICollection<LogEmployeeAttendance> LogEmployeeAttendances { get; set; } = new List<LogEmployeeAttendance>();

    public virtual ICollection<OvertimeWorking> OvertimeWorkings { get; set; } = new List<OvertimeWorking>();

    public virtual ICollection<UserDetailsTable> UserDetailsTables { get; set; } = new List<UserDetailsTable>();
}
