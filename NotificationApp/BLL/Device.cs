namespace BLL


{
    public class Device
    {
        public int DeviceID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int OrganizationID { get; set; }

        public Status DeviceStatus { get; set; }

        public Device(int id, string name, string location, int organizationID, int status)
        {
            DeviceID = id;
            Name = name;
            Location = location;
            OrganizationID = organizationID;
            DeviceStatus = (Status)status;
        }

        public void SetStatus(Status status)
        {
            DeviceStatus = status;
        }
        public override string ToString()
        {
            string devicestring = "";
            devicestring += $"Device ID: {DeviceID.ToString()} \n";
            devicestring += $"Name: {Name} \n";
            devicestring += $"Location: {Location} \n";
            devicestring += $"OrganizationID: {OrganizationID.ToString()} \n";
            devicestring += $"DeviceStatus: {DeviceStatus} \n";

            return devicestring;
        }
    }
}
