namespace Shree_API_AWS.DataTransferObjects
{
    public class ClientDetails_DTO
    {
        public int Id { get; set; }

        public string ClientId { get; set; } = null!;

        public DateTime? DataEnteredOn { get; set; }

        public string? DataEnteredBy { get; set; }

        public string ClientName { get; set; } = null!;

        public string? ClientAddress { get; set; }

        public string? ClientLocation { get; set; }

        public string? ClientServiceType { get; set; }

        public string? SiteManagerName { get; set; }

        public long? SiteManagerContact { get; set; }

        public int? NumberOfLifts { get; set; }

        public DateTime? ContractStartDate { get; set; }

        public DateTime? ContractEndDate { get; set; }

        public int? ContractPeriod { get; set; }

        public decimal? ContractAmount { get; set; }

        public string? TermsOfPayment { get; set; }

        public bool? IsGstIncluded { get; set; }

        public bool? IsActive { get; set; }
    }
}
