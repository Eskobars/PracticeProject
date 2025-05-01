using VueApp1.Server.Models.Matches;

public interface ISoccerService
{
    Task<List<Fixture>> GetTodaysFixturesAsync(DateTime date);
}