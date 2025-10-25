
using BuberDinner.Application.Dinners.Queries.ListUserDinners;
using BuberDinner.Contracts.Dinners;
using Microsoft.AspNetCore.Authorization;

namespace BuberDinner.Api.Controllers;

[ApiController]
[Route("api/v1/dinners")]
[Authorize]
public class DinnerController : Controller
{
    private readonly ILogger<DinnerController> _logger;
    private readonly ISender _mediator;

    public DinnerController(ILogger<DinnerController> logger, ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    [HttpGet("privacy")]
    public IActionResult Privacy()
    {
        return Ok();
    }

    [HttpGet("{id:guid}")]
    public IActionResult GetById([FromRoute] Guid id)
    {
        return  Ok();
    }
    [HttpPost("create")]

    public ActionResult Create([FromBody] DinnerRequest dinner)
    {  
        _logger.LogInformation("Dinner creation sucesfully");
        return Ok();
    }
    [HttpGet("list/{id:guid}")]
    public async Task<IActionResult> List(
        [FromRoute] Guid userId,
        [FromQuery] bool active = true)
    {
        var dinners  = await _mediator.Send(new ListUserDinnersQuery(userId, active));
        return Ok(dinners);
    }
}
