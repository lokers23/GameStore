using Microsoft.AspNetCore.Mvc;

namespace GameStore.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GenreController : ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok("hello world");
    }
}