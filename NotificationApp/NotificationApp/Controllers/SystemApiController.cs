using BLL;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using System.Data.SqlClient;

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
            try
            {
                _notificationService.AddNotification(notification);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
                return StatusCode(500, "A database error occurred. Make sure you have provided valid IDs");
            }
            return Ok("Notification added successfully!");
        }

        [HttpPost("devicestatus/{id}/{status}")]
        public IActionResult ChangeDeviceStatus(int id, int status)
        {
            try
            {
                Device device = _deviceService.ChangeStatus(id, status);
                _notificationService.BuildDeviceStatusNotification(device);
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
