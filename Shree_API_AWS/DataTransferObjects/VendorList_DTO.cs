namespace Shree_API_AWS.DataTransferObjects
{
    public class VendorList_DTO
    {
        public int Id { get; set; }

        public string Vendorid { get; set; } = null!;

        public string Vendorname { get; set; } = null!;

        public string? Vendorlocation { get; set; }

        public string? Vendortype { get; set; }

        public string? Gstnumber { get; set; }

        public string? Ownername { get; set; }

        public long? Ownernumber { get; set; }

        public bool IsActive { get; set; }

        public DateTime? Dataenteredon { get; set; }

        public string? Dataenteredby { get; set; }
    }
}
