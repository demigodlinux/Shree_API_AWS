using System;
using System.Collections.Generic;

namespace Shree_API_AWS.Models;

public partial class Employeeloandetail
{
    public int Id { get; set; }

    public string Employeeid { get; set; } = null!;

    public string? Entryformonth { get; set; }

    public DateTime? Dataenteredon { get; set; }

    public string? Dataenteredby { get; set; }

    public DateTime? Loanissuedon { get; set; }

    public decimal? Loanamount { get; set; }

    public int? Totalinstallments { get; set; }

    public decimal? Installmentamount { get; set; }

    public int? Installmentscompleted { get; set; }

    public decimal? Outstandingloanamount { get; set; }

    public DateTime? Lastloanamountpaid { get; set; }

    public bool? Isloanrepayed { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
