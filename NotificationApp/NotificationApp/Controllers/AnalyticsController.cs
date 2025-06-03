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
        }
        public IActionResult Analytics()
        {
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
        }


    }
}
