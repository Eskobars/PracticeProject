using Newtonsoft.Json;

namespace VueApp1.Server.Models.Leagues
{
    public class LeagueDetails
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("country")]
        public string? Country { get; set; }
    }
}
