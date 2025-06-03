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
            var notifications = _notificationService
                .GetNotificationsByOrganization(account.OrganizationId)
                .Where(n => n.PermissionId == 3 || n.PermissionId == 4 || n.PermissionId == 5)
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
            var devices = _deviceService.GetByOrganization(account.OrganizationId);
            var notifications = _notificationService
                .GetNotificationsByOrganization(account.OrganizationId)
                .Where(n => n.PermissionId == 3 || n.PermissionId == 4 || n.PermissionId == 5)
                .ToList();

            var grouped = notifications
                .GroupBy(n => n.DeviceId ?? -1)
                .Select(g => new
                {
                    Device = devices.FirstOrDefault(d => d.DeviceID == g.Key)?.Name ?? "Unassigned",
                    DeviceStatus = g.Count(x => x.PermissionId == 3),
                    Security = g.Count(x => x.PermissionId == 4),
                    Maintenance = g.Count(x => x.PermissionId == 5)
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
            var notifications = _notificationService
                .GetByOrganization(account.OrganizationId)
                .Where(n => n.PermissionId == 3 || n.PermissionId == 4 || n.PermissionId == 5)
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
                            "rgba(255, 99, 132, 0.7)",   // Device Status
                            "rgba(54, 162, 235, 0.7)",   // Security
                            "rgba(255, 206, 86, 0.7)"    // Maintenance
                        }
                    }
                }
            };

            return Json(result);
        }
    }
}
