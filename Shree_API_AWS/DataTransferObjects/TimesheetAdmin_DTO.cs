using Shree_API_AWS.Models;

namespace Shree_API_AWS.DataTransferObjects
{
    public class TimesheetAdmin_DTO
    {
        public int Id { get; set; }

        public string EmployeeId { get; set; } = null!;

        public DateTime DataEnteredOn { get; set; }

        public string? DataEnteredBy { get; set; } = null!;

        public int? TimesheetDetailId { get; set; }

        public TimesheetEmployee_DTO EmployeeTimesheet { get; set; }

    }
}
