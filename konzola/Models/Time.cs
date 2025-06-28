using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace konzola.Models
{
    public class Time
    {
        [JsonPropertyName("EmployeeName")]
        public string Employee { get; set; }
        [JsonPropertyName("StarTimeUtc")]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("EndTimeUtc")]
        public DateTime EndTime { get; set; }
    }
}
