using System.Security;

namespace BLL
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public int OrganizationId { get; set; }

        public Role() { }

        public Role(int id, string name, int organizationId) // Taking from DB
        {
            RoleId = id;
            Name = name;
            OrganizationId = organizationId;
        }
    }
}
