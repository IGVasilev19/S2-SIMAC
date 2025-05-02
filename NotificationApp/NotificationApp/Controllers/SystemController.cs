using Microsoft.AspNetCore.Mvc;

namespace NotificationApp.Controllers
{
    public class SystemController : Controller
    {
        public IActionResult Inbox()
        {
            return View();
        }

        public IActionResult DevicesPanel()
        {
            return View();
        }

        public IActionResult AdminPanel()
        {
            return View();
        }

        public IActionResult AdminCreateEditPanel()
        {
            return View();
        }
    }
}
