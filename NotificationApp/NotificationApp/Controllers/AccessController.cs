using Microsoft.AspNetCore.Mvc;
using NotificationApp.Models;
using System.Diagnostics;
using BLL;
using DAL;
using System.Reflection.Metadata;
using Service;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace NotificationApp.Controllers
{
    public class AccessController : Controller
    {
        private readonly IAccountService accountService;

        public AccessController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogIn(string email, string password)
        {
            Account account = accountService.LogIn(email, password);
            if (account == null)
            {
                // Account not found
                ViewBag.ErrorMessage = "Account not found.";
                return View("Error");
            }

            if (account.Password != password)
            {
                // Invalid password
                ViewBag.ErrorMessage = "Invalid password.";
                return View("Error");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
            };

            var identity = new ClaimsIdentity(claims, "AuthCookie");
            var principal = new ClaimsPrincipal(identity);

            // Successful login
            return RedirectToAction("Inbox", "System");
        }

        [HttpPost]
        public IActionResult SignUp(string name, string email, string password, Role role)
        {
            accountService.SignUp(name, email, password, role);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("AuthCookie");
            return RedirectToAction("Index", "Home");
        }
    }
}