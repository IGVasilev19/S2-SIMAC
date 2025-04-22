using Microsoft.AspNetCore.Mvc;
using BLL;

namespace NotificationApp.Controllers
{
    public class AccountController : Controller
    {
        // private readonly AccountRepository accountRepository;

        // public AccountController(AccountRepository accountRepository)
        // {
        //     this.accountRepository = accountRepository;
        // }

        public IActionResult Index()
        {
            return View();
        }

        // public IActionResult Login(string email, string password)
        // {
        //     Account account = accountRepository.GetByEmail(email);

        //     if (account == null)
        //     {
        //         // Account not found
        //         ViewBag.ErrorMessage = "Account not found.";
        //         return View("Error");
        //     }

        //     if (account.Password != password)
        //     {
        //         // Invalid password
        //         ViewBag.ErrorMessage = "Invalid password.";
        //         return View("Error");
        //     }

        //     // Successful login
        //     return View("Success", account);
        // }

        public IActionResult UpdateAccount(Account account, string name, string email, string password, Role role)
        {
            return View();
        }
        public IActionResult DeleteAccount(Account account)
        {
            return View();
        }
    }
}
