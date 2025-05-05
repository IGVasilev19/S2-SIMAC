using BLL;
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
        private readonly INotificationService _notificationService;

        public SystemController(IAccountService accountService, INotificationService notificationService)
        {
            _accountService = accountService;
            _notificationService = notificationService;
        }

        [Authorize]
        public IActionResult Inbox()
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (accountId != null)
            {
                if (int.TryParse(accountId, out int id))
                {
                    var account = _accountService.GetById(id);
                    var notifications = _notificationService.GetAll();
                    var vmNotifications = new List<NotificationViewModel>();

                    foreach (var notification in notifications)
                    {
                        vmNotifications.Add(new NotificationViewModel
                        {
                            NotificationID = notification.NotificationID,
                            Title = notification.Title,
                            Content = notification.Content,
                            Important = notification.Important,
                            Date = notification.Date.ToString("yyyy-MM-dd HH:mm:ss")
                        });
                    }

                    InboxViewModel vm = new InboxViewModel
                    {
                        AccountId = account.AccountId,
                        AccountName = account.Name,
                        AccountEmail = account.Email,
                        AccountPassword = account.Password,
                        AccountRole = account.AccountRole.ToString(),
                        Notifications = vmNotifications
                    };

                    return View(vm);
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
