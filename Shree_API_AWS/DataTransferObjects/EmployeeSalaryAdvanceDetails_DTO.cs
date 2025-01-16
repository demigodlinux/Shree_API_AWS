namespace Shree_API_AWS.DataTransferObjects
{
    public class EmployeeSalaryAdvanceDetails_DTO
    {
        public int Id { get; set; }

        public string EmployeeId { get; set; } = null!;

        public DateTime DataEnteredOn { get; set; }

        public string DataEnteredBy { get; set; } = null!;

        public decimal AdvanceAmount { get; set; }

        public DateOnly? AmountProcessedOn { get; set; }

        public string? Transactiontype { get; set; }

        public bool? IsAdvanceDeducted { get; set; }

        public string? Remarks { get; set; }

        public bool IsActive { get; set; }
    }
}
