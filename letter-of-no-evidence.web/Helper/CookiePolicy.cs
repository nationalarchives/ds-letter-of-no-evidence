using Newtonsoft.Json;
using System.Web;

namespace letter_of_no_evidence.web.Helper
{  
    public class CookiePolicy
    {
        [JsonProperty("usage")]
        public bool Usage { get; set; }
        [JsonProperty("settings")]
        public bool Settings { get; set; }
        [JsonProperty("essential")]
        public bool Essential { get; set; }
    }
}
