using System.Security;

namespace BLL
{
    public class Role
    {
        public int RoleId { get; set; }
        public string Name { get; set; }

        public Role(int id, string name) // Taking from DB
        {
            RoleId = id;
            Name = name;
        }
    }
}
