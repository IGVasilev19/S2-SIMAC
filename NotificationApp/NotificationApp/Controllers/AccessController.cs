using Microsoft.AspNetCore.Mvc;
using NotificationApp.Models;
using System.Diagnostics;
using BLL;
using DAL;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Service.Interfaces;

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


        //TODO: Implement Viewbag in the front-end
        [HttpPost]
        public async Task<IActionResult> LogIn(string email, string password)
        {
            var account = accountService.LogIn(email, password);

            if (account == null)
            {
                //ModelState.AddModelError("Email", "Account not found.");  
                ViewBag.Error = "Invalid e-mail or password";
                return View("Index", ViewBag.Error);
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