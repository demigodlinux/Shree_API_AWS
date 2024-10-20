namespace Shree_API_AWS.DataTransferObjects
{
    public class Employee_DTO
    {
        public int Id { get; set; }

        public string EmployeeId { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public long? MobileNumber { get; set; }

        public string? Ecname { get; set; }

        public int? Ecnumber { get; set; }

        public string? Position { get; set; }

        public DateTime? HireDate { get; set; }

        public decimal? Salary { get; set; }

        public decimal? Basic { get; set; }

        public decimal? Hra { get; set; }

        public decimal? SpecialAllowance { get; set; }

        public string? NativePlace { get; set; }

        public bool? IsActive { get; set; }
    }
}
