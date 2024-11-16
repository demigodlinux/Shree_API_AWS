using System;
using System.Collections.Generic;

namespace Shree_API_AWS.Models;

public partial class AlertNotification
{
    public int Id { get; set; }

    public DateTime? Dataenteredon { get; set; }

    public string? Dataenteredby { get; set; }

    public string? Notificationmessage { get; set; }

    public bool? Isapproved { get; set; }

    public bool? Issentback { get; set; }

    public bool? Isadminnotification { get; set; }

    public bool? Isemployeenotification { get; set; }

    public bool? Isnotificationactive { get; set; }

    public int? TimesheetId { get; set; }

    public string? Employeeid { get; set; }
}
