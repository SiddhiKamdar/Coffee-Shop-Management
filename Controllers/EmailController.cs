using CoffeeShopManagment.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopManagment.Controllers
{
    //[CheckAccess]
    public class EmailController : Controller
    {
        private readonly IEmailSender _emailSender;

        public EmailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(string toEmail, string subject, string body)
        {
            await _emailSender.SendEmailAsync(toEmail, subject, body);
            TempData["Success"] = "Email sent successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

