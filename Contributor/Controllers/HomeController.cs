using Contributor.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Contributor.Controllers
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ContributorPortal()
        {
            // Your logic here to retrieve user-specific data
            return View();
        }

        public IActionResult Login()
        {
            // Your login logic here...

            // Redirect to the Profile action
            return RedirectToAction("ContributorPortal", "Home");
        }

    }
}