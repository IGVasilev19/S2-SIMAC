using Microsoft.AspNetCore.Mvc;
using NotificationApp.Models;
using System.Diagnostics;
using BLL;
using DAL;
using System.Reflection.Metadata;
using Service;

namespace NotificationApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService accountService;

        public AccountController(AccountService accountService)
        {
            this.accountService = accountService;
        }

        // Register (POST)
        [HttpPost]
        public IActionResult Register(string name, string email, string password, Role role)
        {
            accountService.SignUp(name, email, password, role);
            return RedirectToAction("Login");
        }

        // Login (POST)
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            Account user = accountService.LogIn(email, password);

            if (user != null)
            {
                HttpContext.Session.SetInt32("AccountId", user.AccountId);
                HttpContext.Session.SetString("Name", user.Name);
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid login";
            return View();
        }
    }
}