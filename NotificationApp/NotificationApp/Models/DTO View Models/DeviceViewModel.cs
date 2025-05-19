using BLL;

namespace NotificationApp.Models.DTO_View_Models
{
    public class DeviceViewModel
    {
        public int DeviceID { get; set; }
        public string DeviceName { get; set; }
        public string DeviceLocation { get; set; }
        public int DeviceOrganizationID { get; set; }
        public Status DeviceStatus { get; set; }    
    }
}
