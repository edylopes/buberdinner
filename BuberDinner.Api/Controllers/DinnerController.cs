
using System.Threading.Tasks;
using BuberDinner.Application.Common.Interfaces.Persistence.Dinners;
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var dinner = await _dinnerRepository.GetByIdAsync(id);
        return dinner is null ?
            NotFound() : Ok(dinner);
    }

    [HttpGet("list/{id}")]
    public async Task<IActionResult> List(
        [FromRoute] Guid id,
        [FromQuery] bool onlyActives = false)
    {
        var result = await _dinnerRepository.ListUserDinnersAsync(id, onlyActives);
        return Ok(result);
    }
}
