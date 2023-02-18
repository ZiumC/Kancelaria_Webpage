using GB_Webpage.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
                    TempData["Error"] = "<strong>Formularz zawiera błędy</strong>, e-mail nie został wysłany.";
                    throw new Exception("Model state isn't valid.");
                }
                else
                {
                    TempData["Success"] = "<strong>Wszystko w porządku</strong>, e-mail został wysłany.";
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