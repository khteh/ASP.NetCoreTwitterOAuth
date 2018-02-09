using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NetCoreTwitterOAuth.Data
{
    public class Url
    {
        [JsonProperty("url")]
        public string UrlString { get; set; }
    }
}