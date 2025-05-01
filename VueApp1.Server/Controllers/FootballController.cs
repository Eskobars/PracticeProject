using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SoccerController : ControllerBase
{
    private readonly ISoccerService _soccerService;
    private readonly ILogger<SoccerController> _logger;

    public SoccerController(ISoccerService soccerService, ILogger<SoccerController> logger)
    {
        _soccerService = soccerService;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves today's soccer fixtures filtered by league and status.
    /// </summary>
    /// <returns>A list of soccer fixtures for today.</returns>
    [HttpGet("today")]
    public async Task<IActionResult> GetTodaysGames()
    {
        try
        {
            var fixtures = await _soccerService.GetTodaysFixturesAsync(DateTime.UtcNow);

            if (fixtures == null || !fixtures.Any())
            {
                return NotFound(new { message = "No fixtures found for today." });
            }

            return Ok(new { fixtures });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while fetching today's fixtures.");

            return StatusCode(500, new { error = "An unexpected error occurred. Please try again later." });
        }
    }

}
