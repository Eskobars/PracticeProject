using Newtonsoft.Json;

namespace VueApp1.Server.Models.Teams
{
    public class TeamDetails
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }
    }
}
