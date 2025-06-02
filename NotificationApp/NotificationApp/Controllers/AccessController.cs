using Microsoft.AspNetCore.Mvc;
using NotificationApp.Models;
using System.Diagnostics;
using BLL;
using DAL;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Service.Interfaces;
using Microsoft.Identity.Client;

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
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int id) && id !=1)
                {
                    return RedirectToAction("Inbox", "System");
                }
                else
                {
                    return RedirectToAction("AdminPanel", "Admin");
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(string email, string password)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            var account = accountService.LogIn(email, password);

            if (account == null)
            {
                ViewBag.Error = "Invalid e-mail or password";
                return View("Index");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                new Claim(ClaimTypes.Name, account.Name),
                new Claim(ClaimTypes.Email, account.Email),
            };

            if (account.Email == "admin@gmail.com")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }

            var identity = new ClaimsIdentity(claims, "AuthCookie");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("AuthCookie", principal);

            if (account.Email == "admin@gmail.com")
            {
                return RedirectToAction("AdminPanel", "Admin");
            }

            return RedirectToAction("Inbox", "System");
        }


        [HttpPost]
        public IActionResult SignUp(string name, string email, string password, int organizationId, int roleId)
        {
            accountService.SignUp(name, email, password, organizationId, roleId);
            return RedirectToAction("Index", "Access");
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("AuthCookie");
            return RedirectToAction("Index", "Access");
        }
    }
}