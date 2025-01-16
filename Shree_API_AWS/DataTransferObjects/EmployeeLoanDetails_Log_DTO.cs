namespace Shree_API_AWS.DataTransferObjects
{
    public class EmployeeLoanDetails_Log_DTO
    {
        public int LogId { get; set; }

        public int EmployeeLoanId { get; set; }

        public string EmployeeId { get; set; } = null!;

        public string TransactionType { get; set; } = null!;

        public DateTime TransactionDate { get; set; }

        public DateTime DataEnteredOn { get; set; }

        public string DataEnteredBy { get; set; } = null!;

        public bool? IsLoanAmountDeducted { get; set; }

        public decimal LoanAmount { get; set; }

        public int? PartOfPaymentRemaining { get; set; }
    }
}
