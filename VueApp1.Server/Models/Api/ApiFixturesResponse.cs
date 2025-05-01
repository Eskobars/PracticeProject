using Newtonsoft.Json;
using VueApp1.Server.Models.Matches;

namespace VueApp1.Server.Models.Api
{
    public class ApiFixturesResponse
    {
        [JsonProperty("response")]
        public List<Fixture> Response { get; set; } = new();
    }
}