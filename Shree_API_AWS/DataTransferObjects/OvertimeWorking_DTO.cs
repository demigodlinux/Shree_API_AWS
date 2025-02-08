namespace Shree_API_AWS.DataTransferObjects
{
    public class OvertimeWorking_DTO
    {
        public int Id { get; set; }

        public string EmployeeId { get; set; } = null!;

        public string DataEnteredBy { get; set; } = null!;

        public DateTime DataEnteredOn { get; set; }

        public string EntryFor { get; set; } = null!;

        public DateOnly DateOfOTWorking { get; set; }

        public float OTHoursWorked { get; set; }

        public bool IsActive { get; set; }
    }
}
