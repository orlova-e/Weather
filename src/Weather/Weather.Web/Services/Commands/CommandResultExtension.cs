using Microsoft.AspNetCore.Mvc;

namespace Weather.Web.Services.Commands;

public static class CommandResultExtension
{
    public static IActionResult AsViewResult<T>(
        this Controller controller,
        CommandResult<T> result)
    {
        if (result.IsError)
            return controller.RedirectToAction("Error", "Home");
        
        return controller.View(result.Result);
    }

    public static IActionResult Redirect<T>(
        this Controller controller,
        CommandResult<T> result,
        string action,
        string controllerRoute = "Index")
    {
        if (result.IsError)
            return controller.RedirectToAction("Error", "Home");

        return controller.RedirectToAction(action, controllerRoute);
    }
}