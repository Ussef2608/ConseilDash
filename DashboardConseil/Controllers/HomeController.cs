using System.Diagnostics;
using DashboardConseil.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DashboardConseil.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            return View(); // Maps to About.cshtml
        }

        public IActionResult Histoire()
        {
            return View(); // Maps to Histoire.cshtml
        }

        public IActionResult Team()
        {
            return View(); // Maps to Team.cshtml
        }

        public IActionResult TeamDetaille()
        {
            return View(); // Maps to TeamDetaille.cshtml
        }
        public IActionResult Service()
        {
            return View(); // Maps to Team.cshtml
        }

        public IActionResult ServiceDetaille()
        {
            return View(); // Maps to TeamDetaille.cshtml
        }

        public IActionResult Contact()
        {
            return View(); // Maps to TeamDetaille.cshtml
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize]
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
        public IActionResult Profil()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
