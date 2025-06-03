using BLL;
using Microsoft.AspNetCore.Mvc;
using NotificationApp.Models;
using Service;
using Service.Interfaces;

namespace NotificationApp.Controllers
{
    public class AnalyticsController : Controller
    {
        private readonly INotificationService _notificationService;

        public AnalyticsController(INotificationService notificationService) 
        {
            _notificationService = notificationService;
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
            return View();
        }

    }
}
