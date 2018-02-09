using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.NetCoreTwitterOAuth.Data
{
    public class Entity
    {
        [JsonProperty("urls")]
        public List<Url> Urls { get; set; }

        [JsonProperty("media")]
        public List<Media> Media { get; set; }
    }
}