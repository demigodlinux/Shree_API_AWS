using System;
using System.Collections.Generic;

namespace Shree_API_AWS.Models;

public partial class ClientDetail
{
    public int Id { get; set; }

    public string Clientid { get; set; } = null!;

    public DateTime? Dataenteredon { get; set; }

    public string? Dataenteredby { get; set; }

    public string Clientname { get; set; } = null!;

    public string? Clientaddress { get; set; }

    public string? Clientlocation { get; set; }

    public string? Clientservicetype { get; set; }

    public string? Sitemanagername { get; set; }

    public long? Sitemanagercontact { get; set; }

    public int? Numberoflifts { get; set; }

    public DateTime? Contractstartdate { get; set; }

    public DateTime? Contractenddate { get; set; }

    public int? Contractperiod { get; set; }

    public decimal? Contractamount { get; set; }

    public string? Termsofpayment { get; set; }

    public bool? Isgstincluded { get; set; }

    public bool? Isactive { get; set; }

    public virtual ICollection<Loginventorydatum> Loginventorydata { get; set; } = new List<Loginventorydatum>();
}
