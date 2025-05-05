using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using NotificationApp.Models;
using Service.Interfaces;
using System.Collections.Specialized;
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
                    var notifications = _notificationService.GetByPermission(account.RoleId); // Needs to be PermissionId later this is for testing
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

                    //DATABASE TESTING---------------------------------------------
                    InboxViewModel vm = new InboxViewModel
                    {
                        AccountId = account.AccountId,
                        AccountName = account.Name,
                        AccountEmail = account.Email,
                        AccountPassword = account.Password,
                        AccountOrganization = account.OrganizationId.ToString(),
                        AccountRole = account.RoleId.ToString(),
                        Notifications = vmNotifications
                    };
                    //DATABASE TESTING---------------------------------------------

                    /*InboxViewModel vm = new InboxViewModel
                    {
                        AccountId = account.AccountId,
                        AccountName = account.Name,
                        AccountEmail = account.Email,
                        AccountPassword = account.Password,
                        AccountRole = account.AccountRole.Name,
                        Notifications = vmNotifications
                    };*/

                    return View(vm);
                }
                else
                {
                    return View();
                }
            }
                return View();
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

        public IActionResult AccountPanel()
        {
            return View();
        }
        public IActionResult Analytics()
        {
            return View();
        }

        public IActionResult RolesPanel()
        {
            return View();
        }

        public IActionResult RolesCreateEditPanel()
        {
            return View();
        }
    }
}
