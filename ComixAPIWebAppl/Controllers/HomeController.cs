using Microsoft.AspNetCore.Mvc;

namespace ComixAPIWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Genres()
        {
            return View();
        }

        public IActionResult Comics()
        {
            return View();
        }
        public IActionResult Publishers()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }
    }
}