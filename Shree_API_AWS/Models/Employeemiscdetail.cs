using System;
using System.Collections.Generic;

namespace Shree_API_AWS.Models;

public partial class Employeemiscdetail
{
    public int Id { get; set; }

    public string Employeeid { get; set; } = null!;

    public DateTime Dataenteredon { get; set; }

    public string Dataenteredby { get; set; } = null!;

    public string? Transactiontype { get; set; }

    public decimal Misccashamount { get; set; }

    public DateOnly? Amountprocessedon { get; set; }

    public bool? Iscashdeducted { get; set; }

    public string? Remarks { get; set; }

    public bool Isactive { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
