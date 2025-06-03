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

        public SystemApiController(INotificationService notificationService)
        {
            _notificationService = notificationService;
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
    }
}
