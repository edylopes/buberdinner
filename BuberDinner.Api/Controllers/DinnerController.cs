
namespace BuberDinner.Api.Controllers;

[ApiController]
[Route("api/v1/dinner")]
public class DinnerController : Controller
{
    private readonly ILogger<DinnerController> _logger;

    public DinnerController(ILogger<DinnerController> logger)
    {
        _logger = logger;
    }
    [HttpGet("home")]
    public IActionResult Invites()
    {
        return Ok(Enumerable.Empty<string>());
    }
    [HttpGet("privacy")]
    public IActionResult Privacy()
    {
        return Ok();
    }
}
