using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Shree_API_AWS.Models
{
    public class FetchBlobModel
    {
        public string EmployeeId { get; set; }
        public string SiteName { get; set; }
        public string SiteLocation { get; set; }
        public string ServiceType { get; set; }
    }
}
