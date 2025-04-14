namespace NotificationApp.Models
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Important { get; set; }
        public bool Read {  get; set; }
        public DateTime Date { get; set; }

        public Notification(string title, string content, bool important, bool read, DateTime date) 
        {
            Title = title;
            Content = content;
            Important = important;
            Read = read;
            Date = date;
        }
    }
}
