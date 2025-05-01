using Newtonsoft.Json;
using VueApp1.Server.Models.Teams;

namespace VueApp1.Server.Models.Leagues
{
    public class League
    {
        [JsonProperty("standings")]
        public List<List<Team>> StandingsData { get; set; } = new();
    }
}
