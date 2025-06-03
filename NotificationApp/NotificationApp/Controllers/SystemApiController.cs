using BLL;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace NotificationApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemApiController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IDeviceService _deviceService;

        public SystemApiController(INotificationService notificationService, IDeviceService deviceService)
        {
            _notificationService = notificationService;
            _deviceService = deviceService;
        }

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok("API is working!");
        }

        [HttpPost("notification")]
        public IActionResult PostNotification([FromBody] Notification notification)
        {
            if (notification == null)
            {
                return BadRequest();
            }
            _notificationService.AddNotification(notification);
            return Ok("Notification added successfully!");
        }

        [HttpPost("devicestatus/{id}/{status}")]
        public IActionResult ChangeDeviceStatus(int id, int status)
        {
            if (id <= 0 || status > 1)
            {
                return BadRequest("Invalid device ID or status.");
            }
            Status newStatus = (Status)status;
            _deviceService.ChangeStatus(id, newStatus);
            if (status == 0)
            {
                return Ok($"Device {id} status changed to ONLINE");
            }
            return Ok($"Device {id} status changed to OFFLINE");
        }
    }
}
