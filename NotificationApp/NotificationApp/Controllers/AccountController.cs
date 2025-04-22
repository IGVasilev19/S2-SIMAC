using Microsoft.AspNetCore.Mvc;
using NotificationApp.Models;
using System.Diagnostics;
using BLL;
using DAL;
using System.Reflection.Metadata;

namespace NotificationApp.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult accountpanel()
        {
            return View();
        }
    }
}