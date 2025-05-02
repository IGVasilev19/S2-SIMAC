using Microsoft.AspNetCore.Mvc;
using BLL;
using Service;
using NotificationApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace NotificationApp.Controllers
{
    public class AccountLoginController : Controller
    {
        //private readonly IAccountService accountService;

        //public AccountLoginController(IAccountService accountService)
        //{
        //    this.accountService = accountService;
        //}

        //public IActionResult Index()
        //{
        //    return View();
        //}

        //public IActionResult LogIn(string email, string password)
        //{
        //    Account account = accountService.LogIn(email, password);
        //    if (account == null)
        //    {
        //        // Account not found
        //        ViewBag.ErrorMessage = "Account not found.";
        //        return View("Error");
        //    }

        //    if (account.Password != password)
        //    {
        //        // Invalid password
        //        ViewBag.ErrorMessage = "Invalid password.";
        //        return View("Error");
        //    }

        //    var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
        //    };

        //    var identity = new ClaimsIdentity(claims, "AuthCookie");
        //    var principal = new ClaimsPrincipal(identity);

        //    var vm = new AccountViewModel
        //    {
        //        AccountId = account.AccountId,
        //        Name = account.Name,
        //        Email = account.Email,
        //        Role = account.AccountRole.ToString(),
        //    };
        //    // Successful login
        //    return View("Success", account);
        //}

        //public IActionResult SignUp(string name, string email, string password, Role role)
        //{
        //    accountService.SignUp(name, email, password, role);
        //    return View("Success");
        //}

        //public async Task<IActionResult> LogOut()
        //{
        //    await HttpContext.SignOutAsync("AuthCookie");
        //    return RedirectToAction("Index", "Home");
        //}

        ////public IActionResult UpdateAccount(Account account, string name, string email, string password, Role role)
        ////{
        ////    return View();
        ////}
        ////public IActionResult DeleteAccount(Account account)
        ////{
        ////    return View();
        ////}
    }
}
