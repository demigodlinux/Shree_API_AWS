using System;
using System.Collections.Generic;

namespace Shree_API_AWS.Models;

public partial class Employeeloandetailslog
{
    public int Logid { get; set; }

    public int Employeeloanid { get; set; }

    public string Employeeid { get; set; } = null!;

    public string Transactiontype { get; set; } = null!;

    public DateTime Transactiondate { get; set; }

    public DateTime Dataenteredon { get; set; }

    public string Dataenteredby { get; set; } = null!;

    public bool? Isloanamountdeducted { get; set; }

    public decimal Loanamount { get; set; }

    public int? Partofpaymentremaining { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual Employeeloandetail Employeeloan { get; set; } = null!;
}
