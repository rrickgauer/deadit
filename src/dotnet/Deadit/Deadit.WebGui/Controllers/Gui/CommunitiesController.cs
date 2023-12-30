using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Gui;

[Controller]
[Route("/communities")]
public class CommunitiesController : Controller
{

    [HttpGet]
    public async Task<IActionResult> CommunitiesPageAsync()
    {
        return View("CommunitiesPage");
    }

}
