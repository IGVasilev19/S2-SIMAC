using System.Security.Cryptography.X509Certificates;

namespace BLL
{
    public class Permission
    {
        public int PermissionId { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }

        public Permission(int permissionId, string name, int? parentId)
        {
            PermissionId = permissionId;
            Name = name;
            ParentId = parentId;
        }
    }
}
