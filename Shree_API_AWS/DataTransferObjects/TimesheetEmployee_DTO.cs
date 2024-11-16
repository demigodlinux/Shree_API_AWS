using Shree_API_AWS.Models;

namespace Shree_API_AWS.DataTransferObjects
{
    public class TimesheetEmployee_DTO
    {
        public int Id { get; set; }

        public string EmployeeId { get; set; } = null!;

        public DateTime DataEnteredOn { get; set; }

        public string? DataEnteredBy { get; set; } = null!;

        public string TimesheetDetails { get; set; } = null!;
    }
}
