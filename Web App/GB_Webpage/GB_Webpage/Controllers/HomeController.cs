using GB_Webpage.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GB_Webpage.Controllers
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

        public IActionResult Specializations()
        {
            return View();
        }

        public IActionResult Team()
        {
            return View();
        }

        public IActionResult Cooperation()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult ValidateEmail(ContactModel contact)
        {

            try
            {

                if (!ModelState.IsValid)
                {
                    throw new Exception("Model state isn't valid.");
                }
                else
                {
                    Console.WriteLine("Model state is valid.");
                }

                return RedirectToAction("contact", null);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return RedirectToAction("contact", null);

            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}