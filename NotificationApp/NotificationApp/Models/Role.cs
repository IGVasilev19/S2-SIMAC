using System.Security;

namespace NotificationApp.Models
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
        public Role(int id, string name, List<Permission> permissions)
        {
            RoleId = id;
            Name = name;
            Permissions = permissions;
        }
    }
}
