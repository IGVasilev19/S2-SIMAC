namespace NotificationApp.Models
{
    public class Device
    {
        public string Name { get; set; }
        public string Location { get; set; }

        public Device(string name, string location)
        {
            Name = name;
            Location = location;
        }
    }
}
