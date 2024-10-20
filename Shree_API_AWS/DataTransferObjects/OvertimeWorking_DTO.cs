namespace Shree_API_AWS.DataTransferObjects
{
    public class OvertimeWorking_DTO
    {
        public int Id { get; set; }

        public string EmployeeId { get; set; } = null!;

        public string? EntryForMonth { get; set; }

        public DateTime? DataEnteredOn { get; set; }

        public string? DataEnteredBy { get; set; }

        public bool? IsSundayDuty { get; set; }

        public DateTime? LastSundayDuty { get; set; }

        public int? TotalSundayDuty { get; set; }

        public bool? IsPublicHolidayDuty { get; set; }

        public DateTime? LastPublicHolidayDuty { get; set; }

        public int? TotalPublicHolidayDuty { get; set; }
    }
}
