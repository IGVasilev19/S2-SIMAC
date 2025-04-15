using Microsoft.AspNetCore.Mvc;
using NotificationApp.Models.Repositories;

namespace NotificationApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountRepository accountRepository;

        public AccountController(AccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}
