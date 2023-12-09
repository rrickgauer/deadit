using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Gui;

[Controller]
[Route("")]
public class HomeController : Controller
{

    [HttpGet]
    public async Task<IActionResult> HomePageAsync()
    {
        return View("HomePage");
    }

}
