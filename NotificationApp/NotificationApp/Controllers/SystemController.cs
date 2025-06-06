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
                            Read = _notificationService.HasUserReadNotification(id, notification.NotificationID),
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
        public IActionResult SearchNotifications(InboxViewModel vm) //TODO: Connect? Idk what I'm doing! Will this work? Will it not? Ig we'll find out!
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (accountId != null)
            {
                if (int.TryParse(accountId, out int id))
                {
                    var account = _accountService.GetById(id);

                    var permissions = _permissionService.GetPermissionsByRoleId(account.RoleId);
                    var permissionIds = permissions.Select(p => p.PermissionId).ToList();
                    var notifications = _notificationService.SearchNotifications(vm.Search, account, permissionIds);
                    notifications = _notificationService.FilterNotifications(account, notifications, vm.FilterRead, vm.FilterImportant);

                    var vmNotifications = new List<NotificationViewModel>();

                    foreach (var notification in notifications)
                    {
                        vmNotifications.Add(new NotificationViewModel
                        {
                            NotificationID = notification.NotificationID,
                            Title = notification.Title,
                            Content = notification.Content,
                            Important = notification.Important,
                            Read = _notificationService.HasUserReadNotification(id, notification.NotificationID),
                            Date = notification.Date.ToString("yyyy-MM-dd HH:mm:ss")
                        });
                    }

                    //vm.AccountId = account.AccountId;
                    //vm.AccountName = account.Name;
                    //vm.AccountEmail = account.Email;
                    //vm.AccountPassword = account.Password;
                    //vm.AccountOrganization = account.OrganizationId.ToString();
                    //vm.AccountRole = account.RoleId.ToString();
                    vm.Notifications = vmNotifications;                
                }
            }

            return View("Inbox", vm);
        }

        [HttpPost]
        public IActionResult MarkNotificationAsRead([FromBody] int notificationId) //TODO: Connect to front-end (I am unsure, if this will work)
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                if(!_notificationService.HasUserReadNotification(id, notificationId))
                {
                    _notificationService.MarkNotificationAsRead(id, notificationId);
                }
            }
            return Ok();
        }

        [HttpPost]
        public IActionResult MarkNotificationAsUnread(int notificationId) //TODO: Connect to front-end 
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                _notificationService.MarkNotificationAsUnread(id, notificationId);
            }
            return RedirectToAction("Inbox");
        }

        public IActionResult DevicesPanel()
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                Account account = _accountService.GetById(id);
                IEnumerable<Device> DevicesByOrganization = _deviceService.GetByOrganization(account.OrganizationId);
                //List<DeviceViewModel> vmDevices = new(); //idk what this does???
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

                return View("DevicesPanel", viewmodel);
            }
            return View("DevicesPanel");
        }

        [HttpPost]
        public IActionResult SearchDevices(DevicePanelViewModel vm)
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                Account account = _accountService.GetById(id);
                IEnumerable<Device> filteredDevices = _deviceService.SearchDevices(vm.Search, account.OrganizationId);
                
                vm.Devices = new();
                foreach (Device device in filteredDevices)
                {
                    DeviceViewModel dvm = new DeviceViewModel
                    {
                        DeviceID = device.DeviceID,
                        Name = device.Name,
                        Location = device.Location,
                        OrganizationID = device.OrganizationID,
                        DeviceStatus = device.DeviceStatus
                    };

                    vm.Devices.Add(dvm);
                }

                return View("DevicesPanel", vm);
            }
            return View("DevicesPanel");
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