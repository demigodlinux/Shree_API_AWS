namespace Shree_API_AWS.DataTransferObjects
{
    public class LocationTracker_DTO
    {
        public int Id { get; set; }

        public string Employeeid { get; set; } = null!;

        public DateTime? Dateenteredon { get; set; }

        public string? Dateenteredby { get; set; }

        public string? Locationlog { get; set; }

        public bool? Isactive { get; set; }
    }
}
