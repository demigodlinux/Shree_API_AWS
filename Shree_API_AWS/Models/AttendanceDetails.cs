using Shree_API_AWS.DataTransferObjects;

namespace Shree_API_AWS.Models
{
    public class AttendanceDetails
    {
        public DateTime? CheckinTiming { get; set; }
        public DateTime? CheckoutTiming { get; set; }
        public EmployeeAttendance_DTO? EmployeeAttendanceDetails { get; set; }

    }
}
