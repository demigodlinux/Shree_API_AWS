using System;
using System.Collections.Generic;

namespace Shree_API_AWS.Models;

public partial class Vendorlist
{
    public int Id { get; set; }

    public string Vendorid { get; set; } = null!;

    public string Vendorname { get; set; } = null!;

    public string? Vendorlocation { get; set; }

    public string? Vendortype { get; set; }

    public string? Gstnumber { get; set; }

    public string? Ownername { get; set; }

    public long? Ownernumber { get; set; }

    public DateTime? Dataenteredon { get; set; }

    public string? Dataenteredby { get; set; }

    public bool? Isactive { get; set; }

    public virtual ICollection<Inventorylist> Inventorylists { get; set; } = new List<Inventorylist>();

    public virtual ICollection<Loginventorydatum> Loginventorydata { get; set; } = new List<Loginventorydatum>();
}
