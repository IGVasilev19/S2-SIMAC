using System.Security.Cryptography.X509Certificates;

namespace BLL
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public string Name { get; set; }

        public Permission(int permissionId, string name)
        {
            PermissionId = permissionId;
            Name = name;
        }
    }
}
