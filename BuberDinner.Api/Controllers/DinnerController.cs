using BuberDinner.Application.Common.Interfaces.Persistence.Dinners;
using BuberDinner.Contracts.Dinners;
using Microsoft.AspNetCore.Authorization;


namespace BuberDinner.Api.Controllers;

[ApiController]
[Route("api/v1/dinners")]
[Authorize]
public class DinnerController : Controller
{
    private readonly ILogger<DinnerController> _logger;
    private readonly IDinnerRepository _dinnerRepository;

    public DinnerController(ILogger<DinnerController> logger, IDinnerRepository dinnerRepository)
    {
        _logger = logger;
        _dinnerRepository = dinnerRepository;
    }
    
    [HttpGet("privacy")]
    public IActionResult Privacy()
    {
        return Ok();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var dinner = await _dinnerRepository.GetByIdAsync(id);
        return dinner is null ?
            NotFound() : Ok(dinner);
    }

    [HttpPost("create")]

    public ActionResult Create([FromBody] DinnerRequest dinner)
    {
        return Ok();
    }
    [HttpGet("list/{id:guid}")]
    public IActionResult List(
        [FromRoute] Guid userId,
        [FromQuery] bool onlyActives = false)
    {
        var result =  _dinnerRepository.ListUserDinnersAsync(userId);
        return Ok(result);
    }
}
