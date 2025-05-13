using System.Security.Cryptography.X509Certificates;

namespace BLL
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public string Name { get; set; }
        public int? RoleId { get; set; } // FK to Role

        public Permission(int permissionId, string name)
        {
            PermissionId = permissionId;
            Name = name;
        }

        public Permission(int permissionId, string name, int roleId)
        {
            PermissionId = permissionId;
            Name = name;
            RoleId = roleId;
        }
    }
}
