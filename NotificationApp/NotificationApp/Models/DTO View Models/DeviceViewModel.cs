<<<<<<< HEAD
﻿using BLL;

namespace NotificationApp.Models.DTO_View_Models
=======
using BLL;
using NotificationApp.Models.DTO_View_Models;
using System.ComponentModel.DataAnnotations;

namespace NotificationApp.Models
>>>>>>> main
{
    public class DeviceViewModel
    {
        public int DeviceID { get; set; }
<<<<<<< HEAD
        public string DeviceName { get; set; }
        public string DeviceLocation { get; set; }
        public int DeviceOrganizationID { get; set; }
        public Status DeviceStatus { get; set; }    
=======
        public string Name { get; set; }
        public string Location { get; set; }
        public int OrganizationID { get; set; }

        public Status DeviceStatus { get; set; }
>>>>>>> main
    }
}
