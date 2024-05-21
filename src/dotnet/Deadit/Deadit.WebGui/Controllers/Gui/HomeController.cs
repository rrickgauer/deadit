using Deadit.Lib.Domain.Constants;
using Deadit.Lib.Domain.Enum;
using Deadit.Lib.Domain.Errors;
using Deadit.Lib.Domain.Other;
using Deadit.Lib.Domain.Paging;
using Deadit.Lib.Service.Contracts;
using Deadit.Lib.Service.ViewModels;
using Deadit.WebGui.Controllers.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.Controllers.Gui;

[Controller]
[Route("")]
public class HomeController(IAuthService authService, HomePageVMService vmService) : GuiController, IControllerName
{
    public static string ControllerRedirectName => IControllerName.RemoveControllerSuffix(nameof(HomeController));

    private readonly IAuthService _authService = authService;
    private readonly HomePageVMService _vmService = vmService;

    /// <summary>
    /// deadit.com
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ActionName(nameof(HomePageAsync))]
    public async Task<IActionResult> HomePageAsync([FromQuery] uint? page)
    {
        PaginationPosts pagination = new(page);
        PostSorting sorting = new(PostSortType.New);

        return await GetHomePageAsync(pagination, sorting);
    }

    [HttpGet("top")]
    [ActionName(nameof(HomePageTopAsync))]
    public async Task<IActionResult> HomePageTopAsync([FromQuery] uint? page, [FromQuery] TopPostSort? sort)
    {
        PaginationPosts pagination = new(page);

        PostSorting sorting = new(PostSortType.Top)
        {
            TopSort = sort ?? TopPostSort.Month,
        };

        return await GetHomePageAsync(pagination, sorting);
    }


    private async Task<IActionResult> GetHomePageAsync(PaginationPosts pagination, PostSorting sorting)
    {
        try
        {
            var getViewModel = await _vmService.GetViewModelAsync(new()
            {
                ClientId = ClientId,
                Pagination = pagination,
                PostSorting = sorting,
            });

            if (!getViewModel.Successful)
            {
                return BadRequest(getViewModel);
            }


            return View(GuiPageViewFiles.HomePage, getViewModel.Data);
        }
        catch (ServiceResponseException ex)
        {
            return BadRequest(ex.Response);
        }
    }








    [HttpGet("/logout")]
    [ActionName(nameof(LogoutPage))]
    public IActionResult LogoutPage()
    {
        // clear client session data
        _authService.ClearSessionData(HttpContext.Session);

        // return to the home page
        return LocalRedirect("/");
    }

}
