using System.Security.Principal;

namespace BLL
{
    public class Organization
    {
        public int OrganizationId { get; set; }
        public string Name { get; set; }
        public List<Device> Devices { get; set; }
        public List<Account> Accounts { get; set; }

        public Organization(int id, string name)
        {
            OrganizationId = id;
            Name = name;
            Devices = new List<Device>();
            Accounts = new List<Account>();
        }
        public Organization(int id, string name, List<Device> devices, List<Account> accounts)
        {
            OrganizationId = id;
            Name = name;
            Devices = devices;
            Accounts = accounts;
        }
    }
}
