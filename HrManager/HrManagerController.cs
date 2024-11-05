using HackathonContract.Model;

namespace HackathonHrManager;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/send_wishlist/[controller]")]
public class HrManagerController(HrManagerService hrManagerService) : ControllerBase
{
    [HttpPost]
    public IActionResult CreateWishList(
        [FromQuery] string type,
        [FromBody] Wishlist wishlist)
    {
        hrManagerService.tryStartHackathon(type, wishlist);
        return Ok("Wishlist succesfull proccessing");
    }
}