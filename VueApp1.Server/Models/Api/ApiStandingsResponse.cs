using Newtonsoft.Json;
using VueApp1.Server.Models.Leagues;

namespace VueApp1.Server.Models.Api
{
    public class ApiStandingsResponse
    {
        [JsonProperty("response")]
        public List<Standings> Response { get; set; } = new();
    }
}