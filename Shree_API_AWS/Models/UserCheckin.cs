namespace Shree_API_AWS.Models
{
    public class UserCheckin
    {
        public string EmployeeId { get; set; }
        public bool IsCheckedIn { get; set; }
        public bool IsCheckedOut { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
