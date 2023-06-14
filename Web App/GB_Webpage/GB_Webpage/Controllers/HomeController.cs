using GB_Webpage.Models;
using GB_Webpage.Resources;
using GB_Webpage.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using System.Reflection;

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
            _logger.LogInformation(LogFormatterService.FormatRequest(HttpContext, MethodBase.GetCurrentMethod()?.Name));
            return View();
        }

        public IActionResult Specializations()
        {
            _logger.LogInformation(LogFormatterService.FormatRequest(HttpContext, MethodBase.GetCurrentMethod()?.Name));
            return View();
        }

        public IActionResult Team()
        {
            _logger.LogInformation(LogFormatterService.FormatRequest(HttpContext, MethodBase.GetCurrentMethod()?.Name));
            return View();
        }

        public IActionResult Cooperation()
        {
            _logger.LogInformation(LogFormatterService.FormatRequest(HttpContext, MethodBase.GetCurrentMethod()?.Name));
            return View();
        }

        public IActionResult Contact()
        {
            _logger.LogInformation(LogFormatterService.FormatRequest(HttpContext, MethodBase.GetCurrentMethod()?.Name));
            return View();
        }

        public async Task<IActionResult> NewsAsync()
        {
            _logger.LogInformation(LogFormatterService.FormatRequest(HttpContext, LogFormatterService.GetAsyncMethodName()));

            IEnumerable<ArticleModel>? articles = new List<ArticleModel>();
            try
            {
                string url = _configuration["ApiUrl"];
                articles = await HttpService.DoGetCollection<ArticleModel>(url);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogFormatterService.FormatException(ex, null));
            }

            if (articles != null)
            {
                return View(articles);
            }

            return View();
        }

        public IActionResult ValidateEmail(ContactModel contact)
        {
            _logger.LogInformation(LogFormatterService.FormatRequest(HttpContext, MethodBase.GetCurrentMethod()?.Name));
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = $"{_contact["3.6_leftside_container"]}";
                    throw new Exception("Model isn't valid.");
                }
                else
                {
                    string emailKey = _configuration["EmailKey"];
                    string emailSendsForm = _configuration["EmailSendsForm"];
                    string emailRecivesForm = _configuration["EmailRecivesForm"];

                    SendMailService mailService = new SendMailService(emailKey, emailSendsForm, emailRecivesForm, contact);

                    mailService.SendMail();
                }
                _logger.LogInformation(LogFormatterService.FormatAction(null, "Sended mail", $"Ok, person {contact.Name} has send mail"));
                TempData["Success"] = $"{_contact["3.5_leftside_container"]}";
                return RedirectToAction("contact", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogFormatterService.FormatAction(null, "Mail not sended", null));
                TempData["Error"] = $"{_contact["3.7_leftside_container"]}";
                _logger.LogError(LogFormatterService.FormatException(ex, null));
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