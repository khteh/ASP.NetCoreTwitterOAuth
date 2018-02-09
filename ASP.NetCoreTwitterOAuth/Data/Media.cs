using Newtonsoft.Json;

namespace ASP.NetCoreTwitterOAuth.Data
{
    public class Media
    {
        [JsonProperty("media_url")]
        public string MediaUrl { get; set; }

        [JsonProperty("media_url_https")]
        public string MediaUrlHttps { get; set; }
    }
}