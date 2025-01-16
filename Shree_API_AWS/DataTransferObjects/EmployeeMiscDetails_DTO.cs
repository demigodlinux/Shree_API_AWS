namespace Shree_API_AWS.DataTransferObjects
{
    public class EmployeeMiscDetails_DTO
    {
        public int Id { get; set; }

        public string EmployeeId { get; set; } = null!;

        public DateTime DataEnteredOn { get; set; }

        public string DataEnteredBy { get; set; } = null!;

        public string? TransactionType { get; set; }

        public decimal MiscCashamount { get; set; }

        public DateOnly? AmountProcessedOn { get; set; }

        public bool? IsCashDeducted { get; set; }

        public string? Remarks { get; set; }

        public bool IsActive { get; set; }
    }
}
