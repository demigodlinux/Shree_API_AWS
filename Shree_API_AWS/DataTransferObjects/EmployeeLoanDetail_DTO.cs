namespace Shree_API_AWS.DataTransferObjects
{
    public class EmployeeLoanDetail_DTO
    {
        public int Id { get; set; }

        public string EmployeeId { get; set; } = null!;

        public string? EntryForMonth { get; set; }

        public DateTime? DataEnteredOn { get; set; }

        public string? DataEnteredBy { get; set; }

        public DateTime? LoanIssuedOn { get; set; }

        public decimal? LoanAmount { get; set; }

        public int? TotalInstallments { get; set; }

        public decimal? InstallmentAmount { get; set; }

        public int? InstallmentsCompleted { get; set; }

        public decimal? OutstandingLoanAmount { get; set; }

        public DateTime? LastLoanAmountPaid { get; set; }

        public bool? IsLoanRepayed { get; set; }
    }
}
