using Azure.Identity;
using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using NotificationApp.Models;
using NotificationApp.Models.DTO_View_Models;
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
        private readonly IDeviceService _deviceService;

        public SystemController(IAccountService accountService, INotificationService notificationService, IRoleService roleService, IPermissionService permissionService, IDeviceService deviceService)
        {
            _accountService = accountService;
            _notificationService = notificationService;
            _roleService = roleService;
            _permissionService = permissionService;
            _deviceService = deviceService;
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
                    //OLD//var notifications = _notificationService.GetByPermission(account.RoleId); //shows hardcoded notifications

                    var permissions = _permissionService.GetPermissionsByRoleId(account.RoleId);
                    var permissionIds = permissions.Select(p => p.PermissionId).ToList();
                    var notifications = _notificationService.GetNotificationsForUser(account, permissionIds);
                    

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

        [HttpPost]
        public IActionResult FilterNotifications(InboxViewModel vm) //TODO: Connect? Idk what I'm doing! Will this work? Will it not? Ig we'll find out!
        {
            if (!ModelState.IsValid)
            {
                return View("Inbox", vm);
            }

            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (accountId != null)
            {
                if (int.TryParse(accountId, out int id))
                {
                    var account = _accountService.GetById(id);

                    var permissions = _permissionService.GetPermissionsByRoleId(account.RoleId);
                    var permissionIds = permissions.Select(p => p.PermissionId).ToList();
                    var notifications = _notificationService.SearchNotifications(vm.Search, account, permissionIds);

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
                    vm.Notifications = vmNotifications;
                }
            }

            return View("Inbox", vm);
        }

        public IActionResult DevicesPanel()
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                Account account = _accountService.GetById(id);
                IEnumerable<Device> DevicesByOrganization = _deviceService.GetByOrganization(account.OrganizationId);
                List<DeviceViewModel> vmDevices = new();
                DevicePanelViewModel viewmodel = new DevicePanelViewModel();
                viewmodel.Devices = new();
                foreach (Device device in DevicesByOrganization)
                {
                    DeviceViewModel vm = new DeviceViewModel
                    {
                        DeviceID = device.DeviceID,
                        Name = device.Name,
                        Location = device.Location,
                        OrganizationID = device.OrganizationID,
                        DeviceStatus = device.DeviceStatus
                    };

                    viewmodel.Devices.Add(vm);
                }

                return View(viewmodel);
            }
            return View();
        }

        public IActionResult DevicesCreateEditPanel()
        {
            return View();
        }

        public IActionResult Analytics()
        {
            return View();
        }
    }
}