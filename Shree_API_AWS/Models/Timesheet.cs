using Newtonsoft.Json;

namespace Shree_API_AWS.Models
{
    public class Timesheet
    {
        [JsonProperty("Time Entry")]
        public string TimeEntry { get; set; }

        [JsonProperty("Time Exit")]
        public string TimeExit { get; set; }

        [JsonProperty("Site Name")]
        public string SiteName { get; set; }

        [JsonProperty("Site Location")]
        public string SiteLocation { get; set; }

        [JsonProperty("Service Description")]
        public string ServiceDescription { get; set; }

        [JsonProperty("Service Type")]
        public string ServiceType { get; set; }

        [JsonProperty("Remarks")]
        public string Remarks { get; set; }

        [JsonProperty("Time Spent")]
        public float TimeSpent { get; set; }

        [JsonProperty("Current Time")]
        public DateTime CurrentTime { get; set; }

        [JsonProperty("Location")]
        public string Location { get; set; }
    }
}
