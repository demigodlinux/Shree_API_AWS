namespace Shree_API_AWS.DataTransferObjects
{
    public class IndianPublicHolidays_DTO
    {
        public int HolidayId { get; set; }

        public DateOnly HolidayDate { get; set; }

        public string HolidayName { get; set; } = null!;

        public string HolidayType { get; set; } = null!;

        public string? Region { get; set; }

        public string? Description { get; set; }

        public bool? IsActive { get; set; }
    }
}
