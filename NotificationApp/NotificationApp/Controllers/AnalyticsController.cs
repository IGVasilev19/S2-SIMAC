using BLL;
using Microsoft.AspNetCore.Mvc;
using NotificationApp.Models;
using Service;
using Service.Interfaces;
using System.Security.Claims;

namespace NotificationApp.Controllers
{
    public class AnalyticsController : Controller
    {
<<<<<<< HEAD
        private readonly INotificationService _notificationService;

        public AnalyticsController(INotificationService notificationService) 
        {
            _notificationService = notificationService;
=======
        private readonly IAccountService _accountService;
        private readonly IPermissionService _permissionService;
        private readonly INotificationService _notificationService;
        private readonly IDeviceService _deviceService;

        public AnalyticsController(IAccountService accountService, IPermissionService permissionService, INotificationService notificationService, IDeviceService deviceService) 
        {
            _accountService = accountService;
            _permissionService = permissionService;
            _notificationService = notificationService;
            _deviceService = deviceService;
>>>>>>> main
        }

        public IActionResult GetEventFrequency()
        {
            var events = _eventService.GetAll();

            var grouped = events
                .GroupBy(e => e.Date.Date)
                .Select(g => new
                {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    Crash = g.Count(x => x.Title == "DeviceCrash"),
                    Maint = g.Count(x => x.Title == "Maintenance"),
                    Tamper = g.Count(x => x.Title == "Tampering")
                }).ToList();

            var response = new
            {
                labels = grouped.Select(g => g.Date),
                datasets = new[]
                {
                    new { label = "Device Crash", data = grouped.Select(g => g.Crash) },
                    new { label = "Maintenance", data = grouped.Select(g => g.Maint) },
                    new { label = "Tampering", data = grouped.Select(g => g.Tamper) }
                }
            };

            return Json(response);
        }

        public IActionResult GetEventsPerDevice()
        {
            var events = _eventService.GetAll();

            var grouped = events
                .GroupBy(e => e.DeviceName)
                .Select(g => new { Device = g.Key, Count = g.Count() });

            var response = new
            {
                labels = grouped.Select(g => g.Device),
                datasets = new[]
                {
                    new { label = "Event Count", data = grouped.Select(g => g.Count) }
                }
            };

            return Json(response);
        }

        public IActionResult Analytics()
        {
<<<<<<< HEAD
            return View();
=======
            var devicePanels = new List<DeviceViewModel>();
            var notificationPanels = new List<(NotificationViewModel, int)>();

            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (accountId != null)
            {
                if (int.TryParse(accountId, out int id))
                {
                    var account = _accountService.GetById(id);

                    var permissions = _permissionService.GetPermissionsByRoleId(account.RoleId);
                    List<int> permissionIds = permissions.Select(p => p.PermissionId).ToList();

                    List<Notification> notifications = _notificationService.GetNotificationsForUser(account, permissionIds);
                    var devices = _deviceService.GetByOrganization(account.OrganizationId);


                    return View();
                }
            }

            return View(); // return empty if no account
>>>>>>> main
        }


    }
}
