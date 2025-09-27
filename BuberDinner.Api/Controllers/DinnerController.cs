
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
    public IActionResult Index()
    {
        return View();
    }
    [HttpGet("privacy")]
    public IActionResult Privacy()
    {
        return View();
    }
}
