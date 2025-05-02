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
        public async Task<IActionResult> LogIn(string email, string password)
        {
            var account = accountService.LogIn(email, password);

            if (account == null)
            {
                ModelState.AddModelError("Email", "Account not found.");
                return View("Index");
            }

            if (account.Password == "Invalid password")
            {
                ModelState.AddModelError("Password", "Invalid password");
                return View("Index");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
            };

            var identity = new ClaimsIdentity(claims, "AuthCookie");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("AuthCookie", principal);

            return RedirectToAction("Inbox", "System");
        }


        [HttpPost]
        public IActionResult SignUp(string name, string email, string password, Role role)
        {
            accountService.SignUp(name, email, password, role);
            return RedirectToAction("Index", "Access");
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("AuthCookie");
            return RedirectToAction("Index", "Access");
        }
    }
}