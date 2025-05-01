using Newtonsoft.Json;
using VueApp1.Server.Models.Leagues;
using VueApp1.Server.Models.Teams;

namespace VueApp1.Server.Models.Matches
{
    public class Fixture
    {
        [JsonProperty("fixture")]
        public FixtureDetails? FixtureData { get; set; }

        [JsonProperty("league")]
        public LeagueDetails? LeagueData { get; set; }

        [JsonProperty("teams")]
        public TeamsDetails? Teams { get; set; }

        public List<TeamInfo> TeamInfo { get; set; } = new();
    }
}