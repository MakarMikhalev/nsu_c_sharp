using HackathonContract.Model;

namespace HackathonHrManager;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/send_wishlist")]
public class HrManagerController(HrManagerService hrManagerService) : ControllerBase
{
    private readonly object _lockObj = new();

    [HttpPost]
    public IActionResult ProcessWishlist(
        [FromQuery] string type,
        [FromBody] Wishlist wishlist)
    {
        Console.WriteLine("Catch new request, wishlist: " + wishlist);

        lock (_lockObj)
        {
            hrManagerService.TryStartHackathon(type, wishlist);
        }

        return Ok("Wishlist succesfull processing");
    }
}