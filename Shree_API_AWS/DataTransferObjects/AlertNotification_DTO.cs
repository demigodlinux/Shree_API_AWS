namespace Shree_API_AWS.DataTransferObjects
{
    public class AlertNotification_DTO
    {
        public int Id { get; set; }

        public DateTime? DataEnteredOn { get; set; }

        public string? DataEnteredBy { get; set; }

        public string? NotificationMessage { get; set; }

        public bool? IsApproved { get; set; }

        public bool? IsSentback { get; set; }

        public bool? IsAdminNotification { get; set; }

        public bool? IsEmployeeNotification { get; set; }

        public bool? IsNotificationActive { get; set; }

        public int? TimesheetId { get; set; }

        public string? EmployeeId { get; set; }
    }
}
