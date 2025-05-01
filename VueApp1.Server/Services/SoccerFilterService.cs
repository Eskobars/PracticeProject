using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using VueApp1.Server.Models.Api;
using VueApp1.Server.Models.Leagues;
using VueApp1.Server.Models.Matches;
using VueApp1.Server.Models.Teams;

public class SoccerService : ISoccerService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _memoryCache;
    private readonly IConfiguration _configuration;

    private const string ApiHost = "v3.football.api-sports.io";
    private readonly string _apiKey;
    private readonly string[] AllowedCountries = { "Italy", "England", "Spain", "Germany", "France", "Portugal" };
    private readonly string[] AllowedStatuses = { "NS", "TBD" };
    private const int SeasonYear = 2024;
    private readonly object _cacheLock = new object();

    public SoccerService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _memoryCache = memoryCache;
        _configuration = configuration;
        _apiKey = _configuration["ApiSettings:ApiKey"]
            ?? throw new InvalidOperationException("API key is not configured.");
    }

    public async Task<List<Fixture>> GetTodaysFixturesAsync(DateTime date)
    {
        var today = date.ToString("yyyy-MM-dd");
        var cacheKey = $"todays-soccer-games-{today}";

        lock (_cacheLock)
        {
            if (_memoryCache.TryGetValue(cacheKey, out List<Fixture>? cachedFixtures))
            {
                return cachedFixtures!;
            }
        }

        var fixtures = await FetchFixtures(today);
        if (fixtures == null) return new List<Fixture>();

        var filteredFixtures = FilterFixtures(fixtures, AllowedStatuses, AllowedCountries);
        var uniqueLeagues = filteredFixtures
            .Select(f => f.LeagueData?.Id)
            .Where(id => !string.IsNullOrEmpty(id))
            .Distinct()
            .ToList();

        var standingsByLeague = new Dictionary<string, Standings>();

        foreach (var leagueId in uniqueLeagues)
        {
            var standings = await GetStandingsForLeague(leagueId);
            if (standings != null)
            {
                standingsByLeague[leagueId] = standings;
            }

            await Task.Delay(3000);
        }

        foreach (var fixture in filteredFixtures)
        {
            var leagueId = fixture.LeagueData?.Id;
            if (!string.IsNullOrEmpty(leagueId) && standingsByLeague.TryGetValue(leagueId, out var standings))
            {
                if (fixture.Teams.Home.Id.HasValue && fixture.Teams.Away.Id.HasValue)
                {
                    fixture.TeamInfo = ExtractTeamInfo(
                        standings,
                        fixture.Teams.Home.Id.Value,
                        fixture.Teams.Away.Id.Value
                    );
                }
            }
        }

        // Remove fixtures with incomplete or missing TeamInfo data
        var completeFixtures = filteredFixtures
            .Where(f =>
                f.TeamInfo != null &&
                f.TeamInfo.Count == 2 &&
                f.TeamInfo.All(t => t != null && t.Rank > 0))
            .ToList();

        lock (_cacheLock)
        {
            _memoryCache.Set(cacheKey, completeFixtures, TimeSpan.FromMinutes(60));
        }

        return completeFixtures;
    }


    private async Task<ApiFixturesResponse?> FetchFixtures(string date)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://{ApiHost}/fixtures?date={date}");
        request.Headers.Add("x-rapidapi-host", ApiHost);
        request.Headers.Add("x-rapidapi-key", _apiKey);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<ApiFixturesResponse>(json);
    }

    private async Task<Standings?> GetStandingsForLeague(string leagueId)
    {
        var cacheKey = $"standings-{leagueId}-{SeasonYear}";

        lock (_cacheLock)
        {
            if (_memoryCache.TryGetValue(cacheKey, out Standings? cachedStandings))
            {
                return cachedStandings;
            }
        }

        var request = new HttpRequestMessage(HttpMethod.Get, $"https://{ApiHost}/standings?league={leagueId}&season={SeasonYear}");
        request.Headers.Add("x-rapidapi-host", ApiHost);
        request.Headers.Add("x-rapidapi-key", _apiKey);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var standingsResponse = JsonConvert.DeserializeObject<ApiStandingsResponse>(json);
        var standings = standingsResponse?.Response?.FirstOrDefault();

        lock (_cacheLock)
        {
            if (standings != null)
            {
                _memoryCache.Set(cacheKey, standings, TimeSpan.FromHours(1));
            }
        }

        return standings;
    }

    private List<Fixture> FilterFixtures(ApiFixturesResponse allFixtures, string[] statuses, string[] countries)
    {
        return allFixtures?.Response?
            .Where(f => f != null &&
                        statuses.Contains(f.FixtureData?.Status?.Short ?? string.Empty) &&
                        countries.Contains(f.LeagueData?.Country ?? string.Empty))
            .ToList() ?? new List<Fixture>();
    }

    private List<TeamInfo> ExtractTeamInfo(Standings standings, int homeTeamId, int awayTeamId)
    {
        var teamRanks = new List<TeamInfo>();
        var teamList = standings?.LeagueData?.StandingsData?.FirstOrDefault();
        if (teamList == null) return teamRanks;

        foreach (var team in teamList)
        {
            if (team?.TeamData?.Id == homeTeamId || team?.TeamData?.Id == awayTeamId)
            {
                teamRanks.Add(new TeamInfo
                {
                    Rank = team.Rank,
                    TeamName = team.TeamData.Name ?? "Unknown",
                    Points = team.Points,
                    GoalsDiff = team.GoalsDiff,
                    Form = team.Form ?? "N/A",
                    Status = team.Status ?? "N/A"
                });
            }
        }

        return teamRanks;
    }
}