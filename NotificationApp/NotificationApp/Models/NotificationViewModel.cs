using BLL;

namespace NotificationApp.Models
{
    public class NotificationViewModel
    {
        public int NotificationID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Important { get; set; }
        //public bool Read { get; set; }
        public string Date { get; set; }
        //public List<Notification> Notifications { get; set; }
    }
}
