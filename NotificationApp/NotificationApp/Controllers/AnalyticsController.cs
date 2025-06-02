using BLL;
using Microsoft.AspNetCore.Mvc;
using NotificationApp.Models;
using Service;
using Service.Interfaces;

namespace NotificationApp.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService) 
        {
            _analyticsService = analyticsService;
        }
        public IActionResult Analytics()
        {
            //var devicePanels = new List<DeviceViewModel>();
            //var notificationPanels = new List<(NotificationViewModel, int)>();

            //foreach (Device device in _analyticsService.GetAll())
            //{
            //    // Add device to view model list
            //    devicePanels.Add(new DeviceViewModel
            //    {
            //        DeviceID = device.DeviceID,
            //        Name = device.Name,
            //        Location = device.Location,
            //        OrganizationID = device.OrganizationID,
            //        DeviceStatus = device.DeviceStatus
            //    });

            //    // Add each notification as a tuple with DeviceID
            //    foreach (Notification notification in _analyticsService.GetByDeviceID(device.DeviceID))
            //    {
            //        NotificationViewModel NotificationViewModel = new NotificationViewModel
            //        {
            //            NotificationID = notification.NotificationID,
            //            Title = notification.Title,
            //            Content = notification.Content,
            //            Important = notification.Important,
            //            Date = notification.Date.ToString("yyyy-MM-dd HH:mm")
            //        };

            //        notificationPanels.Add((NotificationViewModel, device.DeviceID));
            //    }
            //}

            //var viewModel = new AnalyticsViewModel
            //{
            //    DevicePanels = devicePanels,
            //    NotificationPanels = notificationPanels
            //};

            return View(viewModel);
        }

    }
}
