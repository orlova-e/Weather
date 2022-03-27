using Microsoft.AspNetCore.Mvc;

namespace Weather.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}