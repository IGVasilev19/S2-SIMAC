using BLL;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System.Data.SqlClient;
using Microsoft.AspNetCore.SignalR;
using NotificationApp.Hubs;

namespace NotificationApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemApiController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IDeviceService _deviceService;
        private readonly IHubContext<NotificationHub> _hubContext;

        public SystemApiController(INotificationService notificationService, IDeviceService deviceService, IHubContext<NotificationHub> hubContext)
        {
            _notificationService = notificationService;
            _deviceService = deviceService;
            _hubContext = hubContext;
        }

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok("API is working!");
        }

        [HttpPost("notification")]
        public async Task<IActionResult> PostNotification([FromBody] Notification notification)
        {
            if (notification == null)
                return BadRequest();

            try
            {
                _notificationService.AddNotification(notification);

                Console.WriteLine($"Sending to group {notification.OrganizationId}");

                await _hubContext.Clients.Group(notification.OrganizationId.ToString())
                .SendAsync("ReceiveNotification", new {
                    notificationId = notification.NotificationID,
                    title = notification.Title,
                    date = notification.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                    content = notification.Content,
                    important = notification.Important
                });
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return StatusCode(500, "Database error");
            }

            return Ok("Notification added successfully!");
        }

        [HttpPost("devicestatus/{id}/{status}")]
        public IActionResult ChangeDeviceStatus(int id, int status)
        {
            try
            {
                _deviceService.ChangeStatus(id, status);
            }
            catch(Exception ex)
            {
                if (ex is ArgumentException || ex is KeyNotFoundException)
                {
                    return BadRequest(ex.Message);
                }
                else
                {
                    throw;
                }
            }

            if (status == 0)
            {
                return Ok($"Device {id} status changed to ONLINE");
            }
            return Ok($"Device {id} status changed to OFFLINE");
        }
    }
}
