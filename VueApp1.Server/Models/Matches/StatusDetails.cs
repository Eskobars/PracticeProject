using Newtonsoft.Json;

namespace VueApp1.Server.Models.Matches
{
    public class StatusDetails
    {
        [JsonProperty("short")]
        public string? Short { get; set; }
    }
}
