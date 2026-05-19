using GB_Webpage.Models;
using GB_Webpage.Resources;
using GB_Webpage.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

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
            _logger.LogInformation(LogFormatterService.FormatRequest(HttpContext, LogFormatterService.GetMethodName()));
            return View();
        }

        public IActionResult Specializations()
        {
            _logger.LogInformation(LogFormatterService.FormatRequest(HttpContext, LogFormatterService.GetMethodName()));

            string? selectedLanguage;
            string? cultureCookie = HttpContext.Request.Cookies[".AspNetCore.Culture"];

            if (cultureCookie is not null)
            {
                selectedLanguage = cultureCookie.Split("=")[1].Split("|")[0];
            }
            else
            {
                selectedLanguage = "pl-PL";
            }

            ViewBag.SelectedLanguage = selectedLanguage;

            return View();
        }

        public IActionResult Team()
        {
            _logger.LogInformation(LogFormatterService.FormatRequest(HttpContext, LogFormatterService.GetMethodName()));

            string? selectedLanguage;
            string? cultureCookie = HttpContext.Request.Cookies[".AspNetCore.Culture"];

            if (cultureCookie is not null)
            {
                selectedLanguage = cultureCookie.Split("=")[1].Split("|")[0];
            }
            else
            {
                selectedLanguage = "pl-PL";
            }

            ViewBag.SelectedLanguage = selectedLanguage;

            return View();
        }

        public IActionResult Cooperation()
        {
            _logger.LogInformation(LogFormatterService.FormatRequest(HttpContext, LogFormatterService.GetMethodName()));
            return View();
        }

        public IActionResult Contact()
        {
            _logger.LogInformation(LogFormatterService.FormatRequest(HttpContext, LogFormatterService.GetMethodName()));
            return View();
        }

        public ActionResult CaptchaImage()
        {
            string captcha = GenerateCaptchaText();
            HttpContext.Session.SetString("Captcha", captcha);

            using (Bitmap bmp = new Bitmap(120, 40))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);

                using (System.Drawing.Font font = new System.Drawing.Font("Arial", 20, FontStyle.Bold))
                {
                    g.DrawString(captcha, font, Brushes.Black, 10, 5);
                }

                // Optional noise
                var rand = new Random();
                for (int i = 0; i < 20; i++)
                {
                    g.DrawEllipse(Pens.Gray, rand.Next(0, 120), rand.Next(0, 40), 2, 2);
                }

                using (var ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Png);
                    return File(ms.ToArray(), "image/png");
                }
            }
        }

        public async Task<IActionResult> NewsAsync()
        {
            _logger.LogInformation(LogFormatterService.FormatRequest(HttpContext, LogFormatterService.GetMethodName()));

            IEnumerable<ArticleModel>? articles = new List<ArticleModel>();
            try
            {
                string url = _configuration["ApplicationSettings:ArticlesSettings:ArticlesApiUrl"];
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

        public IActionResult Privacy()
        {
            _logger.LogInformation(LogFormatterService.FormatRequest(HttpContext, LogFormatterService.GetMethodName()));
            return View();
        }

        public IActionResult ValidateEmail(ContactModel contact)
        {
            var captcha = HttpContext.Session.GetString("Captcha");
            _logger.LogInformation(LogFormatterService.FormatRequest(HttpContext, LogFormatterService.GetMethodName()));


            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["Error"] = $"{_contact["3.6_leftside_container"]}";
                    throw new Exception("Model isn't valid.");
                }
                else
                {
                    //recaptcha here
                    if (captcha != contact.Captcha)
                    {
                        throw new Exception("Invalid captcha");
                    }


                    string emailKey = _configuration["ApplicationSettings:FormSettings:EmailSenderKey"];
                    string emailSendsForm = _configuration["ApplicationSettings:FormSettings:EmailSender"];
                    string emailRecivesForm = _configuration["ApplicationSettings:FormSettings:EmailReceiver"];

                    SendMailService mailService = new SendMailService(emailKey, emailSendsForm, emailRecivesForm, contact);

                    mailService.SendMail();
                }
                _logger.LogInformation(LogFormatterService.FormatAction("Sended mail", $"Ok, person {contact.Name} has send mail", null));
                TempData["Success"] = $"{_contact["3.5_leftside_container"]}";
                return RedirectToAction("contact", null);
            }
            catch (Exception ex)
            {
                _logger.LogError(LogFormatterService.FormatAction("Mail not sended", null, null));
                TempData["Error"] = $"{_contact["3.7_leftside_container"]}";
                _logger.LogError(LogFormatterService.FormatException(ex, null));
                return RedirectToAction("contact", null);
            }
        }

        [Route("Error/{statusCode}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statusCode)
        {
            _logger.LogInformation(LogFormatterService.FormatRequest(HttpContext, LogFormatterService.GetMethodName()));
            return View(
                new ErrorViewModel
                {
                    RequestId = HttpContext.TraceIdentifier,
                    ActivtyId = Activity.Current?.Id,
                    StatusCode = statusCode
                });
        }

        private string GenerateCaptchaText(int length = 6)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}