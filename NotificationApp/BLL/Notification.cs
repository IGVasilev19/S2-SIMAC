namespace BLL
{
    public class Notification
    {
        public int NotificationID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Important { get; set; }
        public bool Read {  get; set; }
        public DateTime Date { get; set; }

        public Notification(int id, string title, string content, bool important, bool read, DateTime date) 
        {
            NotificationID = id;
            Title = title;
            Content = content;
            Important = important;
            Read = read;
            Date = date;
        }
    }
}
