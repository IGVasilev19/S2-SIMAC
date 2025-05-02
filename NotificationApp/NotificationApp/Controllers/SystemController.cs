using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using NotificationApp.Models;
using Service;
using System.Security.Claims;

namespace NotificationApp.Controllers
{
    public class SystemController : Controller
    {
        private readonly IAccountService _accountService;

        public SystemController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Authorize]
        public IActionResult Inbox()
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(accountId != null)
            {
                if (!int.TryParse(accountId, out int id))
                {
                    var account = _accountService.GetById(id);

                    var vm = new AccountViewModel
                    {
                        AccountId = account.AccountId,
                        Name = account.Name,
                        Email = account.Email,
                        Role = account.AccountRole.ToString()
                    };
                    return View();
                }
                else
                {
                    return View();
                }
            }
            return View("Error");
        }
        [Authorize]
        public IActionResult DevicesPanel()
        {
            return View();
        }
        [Authorize]
        public IActionResult AdminPanel()
        {
            return View();
        }
        [Authorize]
        public IActionResult AdminCreateEditPanel()
        {
            return View();
        }
        [Authorize]
        public IActionResult AccountPanel()
        {
            return View();
        }
    }
}
