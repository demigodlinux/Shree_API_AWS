using System;
using System.Collections.Generic;

namespace Shree_API_AWS.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string Employeeid { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string? Lastname { get; set; }

    public DateTime? Dateofbirth { get; set; }

    public long? Mobilenumber { get; set; }

    public string? Ecname { get; set; }

    public long? Ecnumber { get; set; }

    public string? Position { get; set; }

    public DateTime? Hiredate { get; set; }

    public decimal? Salary { get; set; }

    public decimal? Basic { get; set; }

    public decimal? Hra { get; set; }

    public decimal? Specialallowance { get; set; }

    public string? Nativeplace { get; set; }

    public bool? Isactive { get; set; }

    public virtual ICollection<Employeeattendance> Employeeattendances { get; set; } = new List<Employeeattendance>();

    public virtual ICollection<Employeeloandetail> Employeeloandetails { get; set; } = new List<Employeeloandetail>();

    public virtual ICollection<LogEmployeeattendance> LogEmployeeattendances { get; set; } = new List<LogEmployeeattendance>();

    public virtual ICollection<Overtimeworking> Overtimeworkings { get; set; } = new List<Overtimeworking>();

    public virtual ICollection<Userdetailstable> Userdetailstables { get; set; } = new List<Userdetailstable>();
}
