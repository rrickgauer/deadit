using Microsoft.AspNetCore.Mvc;

namespace Deadit.WebGui.ViewComponents;

public abstract class ViewComp<T> : ViewComponent
{
    public abstract string RazorFileName { get; }

    protected string ViewFile => $"~/Views/Shared/Components/{RazorFileName}.cshtml";

    public async Task<IViewComponentResult> InvokeAsync(T model)
    {
        return View(ViewFile, model);
    }

}