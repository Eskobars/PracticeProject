using Newtonsoft.Json;

namespace VueApp1.Server.Models.Teams
{
    public class TeamsDetails
    {
        [JsonProperty("home")]
        public TeamDetails? Home { get; set; }

        [JsonProperty("away")]
        public TeamDetails? Away { get; set; }
    }
}
