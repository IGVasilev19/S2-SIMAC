using System.Security;

namespace BLL
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public List<Permission> Permissions { get; set; }

        public Role(string name, List<Permission> permissions)
        {
            Name = name;
            Permissions = permissions;
        }
        public Role(int id, string name) // Taking from DB
        {
            RoleId = id;
            Name = name;
            Permissions = new List<Permission>();
        }

        public Role(int id, string name, List<Permission> permissions) // Taking from DB
        {
            RoleId = id;
            Name = name;
            Permissions = permissions;
        }
    }
}
