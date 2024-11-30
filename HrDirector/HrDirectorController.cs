using HackathonContract.Model;

namespace HackathonHrDirector;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/send_hackathon")]
public class HrDirectorController(HrDirectorService hrDirectorService) : ControllerBase
{
    [HttpPost]
    public IActionResult ProcessingHackathon([FromBody] HackathonResult hackathonResult)
    {
        Console.WriteLine("Catch new request, hackathon result: " + hackathonResult);
        hrDirectorService.SummingUp(hackathonResult);
        return Ok("Successful processing hackathon");
    }
}