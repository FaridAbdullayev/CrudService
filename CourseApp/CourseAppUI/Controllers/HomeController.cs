
using CourseAppUI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CourseAppUI.Controllers
{
    [ServiceFilter(typeof(AuthFilter))]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
