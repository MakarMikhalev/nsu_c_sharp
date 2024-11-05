using HackathonContract.Model;

namespace HackathonHrDirector;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/send_hackathon/[controller]")]
public class HrDirectorController(HrDirectorService hrDirectorService) : ControllerBase
{
    [HttpPost]
    public IActionResult ProcessingHackathon([FromBody] HackathonResult hackathonResult)
    {
        hrDirectorService.SummingUp(hackathonResult);
        return Ok("Successful processing hackathon");
    }
}