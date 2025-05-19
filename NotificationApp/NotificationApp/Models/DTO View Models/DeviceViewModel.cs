using BLL;
using NotificationApp.Models.DTO_View_Models;
using System.ComponentModel.DataAnnotations;

namespace NotificationApp.Models
{
    public class DeviceViewModel
    {
        public int DeviceID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int OrganizationID { get; set; }

        public Status DeviceStatus { get; set; }
    }
}
