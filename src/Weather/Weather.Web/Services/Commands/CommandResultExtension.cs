using Microsoft.AspNetCore.Mvc;

namespace Weather.Web.Services.Commands;

public static class CommandResultExtension
{
    public static IActionResult AsViewResult<T>(
        this Controller controller,
        CommandResult<T> result)
    {
        return controller.View(result);
    }
}