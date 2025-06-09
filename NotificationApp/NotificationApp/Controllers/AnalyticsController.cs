using BLL;
using Microsoft.AspNetCore.Mvc;
using NotificationApp.Models;
using Service.Interfaces;
using System.Security.Claims;

namespace NotificationApp.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IPermissionService _permissionService;
        private readonly INotificationService _notificationService;
        private readonly IDeviceService _deviceService;

        public AnalyticsController(
            IAccountService accountService,
            IPermissionService permissionService,
            INotificationService notificationService,
            IDeviceService deviceService)
        {
            _accountService = accountService;
            _permissionService = permissionService;
            _notificationService = notificationService;
            _deviceService = deviceService;
        }

        [Permission("Analytics Access")]
        public IActionResult Analytics()
        {
            return View();
        }

        public JsonResult GetEventFrequency()
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(accountId, out int id))
                return Json(null);

            var account = _accountService.GetById(id);
            var cutoffDate = DateTime.Now.AddDays(-30);

            var notifications = _notificationService
                .GetNotificationsByOrganization(account.OrganizationId)
                .Where(n => (n.PermissionId == 3 || n.PermissionId == 4 || n.PermissionId == 5) && n.Date >= cutoffDate)
                .ToList();

            var grouped = notifications
                .GroupBy(n => n.Date.Date)
                .Select(g => new
                {
                    Date = g.Key.ToString("yyyy-MM-dd"),
                    DeviceStatus = g.Count(x => x.PermissionId == 3),
                    Security = g.Count(x => x.PermissionId == 4),
                    Maintenance = g.Count(x => x.PermissionId == 5)
                })
                .ToList();

            var result = new
            {
                labels = grouped.Select(g => g.Date),
                datasets = new[]
                {
                    new { label = "Device Status", data = grouped.Select(g => g.DeviceStatus) },
                    new { label = "Security", data = grouped.Select(g => g.Security) },
                    new { label = "Maintenance", data = grouped.Select(g => g.Maintenance) }
                }
            };

            return Json(result);
        }


        public JsonResult GetEventsPerDevice()
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(accountId, out int id))
                return Json(null);

            var account = _accountService.GetById(id);
            var cutoffDate = DateTime.Now.AddDays(-30);

            var notifications = _notificationService
                .GetNotificationsByOrganization(account.OrganizationId)
                .Where(n => (n.PermissionId == 3 || n.PermissionId == 4 || n.PermissionId == 5) && n.Date >= cutoffDate)
                .ToList();

            var devices = _deviceService.GetByOrganization(account.OrganizationId);

            var grouped = notifications
                .GroupBy(n => n.DeviceId ?? -1)
                .Select(g =>
                {
                    string deviceName = g.Key == -1
                        ? "Unassigned"
                        : devices.FirstOrDefault(d => d.DeviceID == g.Key)?.Name ?? "Unknown";

                    return new
                    {
                        Device = deviceName,
                        DeviceStatus = g.Count(x => x.PermissionId == 3),
                        Security = g.Count(x => x.PermissionId == 4),
                        Maintenance = g.Count(x => x.PermissionId == 5)
                    };
                })
                .ToList();

            var result = new
            {
                labels = grouped.Select(g => g.Device),
                datasets = new[]
                {
                    new { label = "Device Status", data = grouped.Select(g => g.DeviceStatus) },
                    new { label = "Security", data = grouped.Select(g => g.Security) },
                    new { label = "Maintenance", data = grouped.Select(g => g.Maintenance) }
                }
            };

            return Json(result);
        }



        public JsonResult GetEventTypeDistribution()
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(accountId, out int id))
                return Json(null);

            var account = _accountService.GetById(id);
            var cutoffDate = DateTime.Now.AddDays(-30);

            var notifications = _notificationService
                .GetNotificationsByOrganization(account.OrganizationId)
                .Where(n => (n.PermissionId == 3 || n.PermissionId == 4 || n.PermissionId == 5) && n.Date >= cutoffDate)
                .ToList();

            var result = new
            {
                labels = new[] { "Device Status", "Security", "Maintenance" },
                datasets = new[]
                {
                    new
                    {
                        data = new[]
                        {
                            notifications.Count(x => x.PermissionId == 3),
                            notifications.Count(x => x.PermissionId == 4),
                            notifications.Count(x => x.PermissionId == 5)
                        },
                        backgroundColor = new[]
                        {
                            "rgb(67, 148, 229)",    // Device Status
                            "rgb(202, 70, 70)" ,   // Security
                            "rgb(135, 111, 212)"     // Maintenance
                        }
                    }
                }
            };

            return Json(result);
        }

    }
}
