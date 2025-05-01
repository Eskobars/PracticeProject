using Newtonsoft.Json;

namespace VueApp1.Server.Models.Leagues
{
    public class Standings
    {
        [JsonProperty("league")]
        public League? LeagueData { get; set; }
    }
}
