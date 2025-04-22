namespace BLL
{
    public class Device
    {
        public int DeviceID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        public Status DeviceStatus { get; set; }

        public Device(int id, string name, string location)
        {
            DeviceID = id;
            Name = name;
            Location = location;
        }
    }
}
