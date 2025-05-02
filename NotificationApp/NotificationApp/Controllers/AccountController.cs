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
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
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

            var vm = new AccountViewModel
            {
                AccountId = account.AccountId,
                Name = account.Name,
                Email = account.Email,
                Role = account.AccountRole.ToString(),
            };
            // Successful login
            return View("Success", vm);
        }

        [HttpPost]
        public IActionResult SignUp(string name, string email, string password, Role role)
        {
            accountService.SignUp(name, email, password, role);
            return View("Success");
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("AuthCookie");
            return RedirectToAction("Index", "Home");
        }

        //public IActionResult UpdateAccount(Account account, string name, string email, string password, Role role)
        //{
        //    return View();
        //}
        //public IActionResult DeleteAccount(Account account)
        //{
        //    return View();
        //}
    }
}