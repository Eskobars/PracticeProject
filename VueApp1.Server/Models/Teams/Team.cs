using Newtonsoft.Json;

namespace VueApp1.Server.Models.Teams
{
    public class Team
    {
        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("team")]
        public TeamDetails? TeamData { get; set; }

        [JsonProperty("points")]
        public int Points { get; set; }

        [JsonProperty("goalsDiff")]
        public int GoalsDiff { get; set; }

        [JsonProperty("form")]
        public string? Form { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }
    }
}
