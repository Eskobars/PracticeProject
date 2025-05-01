using Newtonsoft.Json;

namespace VueApp1.Server.Models.Matches
{
    public class FixtureDetails
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("status")]
        public StatusDetails? Status { get; set; }
    }
}
