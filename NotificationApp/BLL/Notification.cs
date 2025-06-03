using Microsoft.CodeAnalysis;
using System.Text.Json.Serialization;

namespace BLL
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Important { get; set; }
        public int? OrganizationId { get; set; }
        public int? PermissionId { get; set; }
        public int? DeviceId { get; set; }
        public DateTime Date { get; set; }

        public Notification(int id, string title, string content, bool important, int organizationId, DateTime date) 
        {
            NotificationID = id;
            Title = title;
            Content = content;
            Important = important;
            OrganizationId = organizationId;
            Date = date;
        }
        public Notification()
        {
            //This is used for JSON deserialization
        }
    }
}
