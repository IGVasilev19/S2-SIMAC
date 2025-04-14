using System.Security.Principal;

namespace NotificationApp.Models
{
    public class Organization
    {
        public string Name { get; set; }
        public List<Device> Devices { get; set; }
        public List<Account> Accounts { get; set; }

        public Organization(string name)
        {
            Name = name;
            Devices = new List<Device>();
            Accounts = new List<Account>();
        }
        public Organization(string name, List<Device> devices, List<Account> accounts)
        {
            Name = name;
            Devices = devices;
            Accounts = accounts;
        }
    }
}
