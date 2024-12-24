using System;
using System.Collections.Generic;

namespace Shree_API_AWS.Models;

public partial class Loginventorydatum
{
    public int Id { get; set; }

    public string Materialid { get; set; } = null!;

    public string? Typeoftransaction { get; set; }

    public DateTime? Dateoftransaction { get; set; }

    public string? Materialname { get; set; }

    public string? Materialdescription { get; set; }

    public string? Vendorid { get; set; }

    public string? Clientid { get; set; }

    public string? Typeofmaterial { get; set; }

    public decimal? Quantity { get; set; }

    public string? Measuringunit { get; set; }

    public decimal? Currentmaterialpriceperunit { get; set; }

    public decimal? Priceofunittransacted { get; set; }

    public DateTime? Dateenteredon { get; set; }

    public string? Dateenteredby { get; set; }

    public virtual ClientDetail? Client { get; set; }

    public virtual Vendorlist? Vendor { get; set; }
}
