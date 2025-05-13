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
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;

        public SystemController(IAccountService accountService, INotificationService notificationService, IRoleService roleService, IPermissionService permissionService)
        {
            _accountService = accountService;
            _notificationService = notificationService;
            _roleService = roleService;
            _permissionService = permissionService;
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
        
        public IActionResult DevicesPanel()
        {
            return View();
        }
        public IActionResult DevicesCreateEditPanel()
        {
            return View();
        }
        public IActionResult AdminPanel()
        {
            return View();
        }
        
        public IActionResult AdminCreateEditPanel()
        {
            return View();
        }

        public IActionResult AccountPanel()
        {
            return View();
        }
        public IActionResult AccountCreateEditPanel()
        {
            return View();
        }
        public IActionResult Analytics()
        {
            return View();
        }

        public IActionResult RolesPanel()
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (accountId != null)
            {
                if (int.TryParse(accountId, out int id))
                {
                    var account = _accountService.GetById(id);
                    List<Role> allRolesInOrg = (List<Role>)_roleService.GetAll(); //Call all roles by org id
                    RolesCreateEditPanelViewModel vm = new RolesCreateEditPanelViewModel();
                    vm.RoleId = null;
                    foreach(var role in allRolesInOrg)
                    {
                            vm.RoleId = role.RoleId;
                    }


                    return View(vm);
                }
                else
                {
                    return View();
                }
            }
            return View();
        }

        public IActionResult RolesCreateEditPanel()
        {
            return View();
        }
    }
}
