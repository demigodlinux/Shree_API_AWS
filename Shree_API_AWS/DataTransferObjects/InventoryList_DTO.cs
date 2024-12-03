namespace Shree_API_AWS.DataTransferObjects
{
    public class InventoryList_DTO
    {
        public int Id { get; set; }

        public string Materialid { get; set; } = null!;

        public string? Materialname { get; set; }

        public string? Materialdescription { get; set; }

        public string? Vendorid { get; set; }

        public string? Typeofmaterial { get; set; }

        public decimal? Quantity { get; set; }

        public string? Measuringunit { get; set; }

        public decimal? Materialpriceperunit { get; set; }

        public DateTime? Dateenteredon { get; set; }

        public string? Dateenteredby { get; set; }
    }
}
