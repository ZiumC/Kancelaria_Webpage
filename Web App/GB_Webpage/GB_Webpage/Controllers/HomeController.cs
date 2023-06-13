using GB_Webpage.Models;
using GB_Webpage.Resources;
using GB_Webpage.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;

namespace GB_Webpage.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStringLocalizer<Contact> _contact;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, IStringLocalizer<Contact> contact, IConfiguration configuration)
        {
            _logger = logger;
            _contact = contact;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            _logger.LogInformation(LogBuilder.BuildLogFrom(HttpContext, MethodBase.GetCurrentMethod()?.Name));
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

        public async Task<IActionResult> NewsAsync()
        {
            string url = _configuration.GetValue<string>("ApiUrl");

            var articles = await HttpService.DoGetCollection<ArticleModel>(url);

            if (articles != null)
            {

                return View(articles);

            }

            return View();

        }

        public async Task<IActionResult> ValidateEmail(ContactModel contact)
        {
            Console.WriteLine(Request.Cookies[CookieRequestCultureProvider.DefaultCookieName]);

            try
            {

                if (!ModelState.IsValid)
                {

                    TempData["Error"] = $"{_contact["3.6_leftside_container"]}";
                    throw new Exception("Model state isn't valid.");
                }
                else
                {

                    string emailKey = _configuration.GetValue<string>("EmailKey");
                    string emailSendsForm = _configuration.GetValue<string>("EmailSendsForm");
                    string emailRecivesForm = _configuration.GetValue<string>("EmailRecivesForm");


                    SendMailService mailService = new SendMailService(emailKey, emailSendsForm, emailRecivesForm, contact);

                    bool isMailSent = await mailService.sendMailAsync();

                    if (isMailSent)
                    {
                        TempData["Success"] = $"{_contact["3.5_leftside_container"]}";
                    }
                    else
                    {
                        TempData["Error"] = $"{_contact["3.7_leftside_container"]}";
                    }
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