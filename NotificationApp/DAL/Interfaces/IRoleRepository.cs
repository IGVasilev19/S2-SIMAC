using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;

namespace DAL.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        void AssignPermission(Role role, IEnumerable<Permission> permissions);
        List<Permission> GetPermissionsByRoleId(int roleId);
        void AssignPermission(int roleId, IEnumerable<Permission> permissions);

        IEnumerable<Role> GetAllRolesByOrganisationId(int organizationId);
    }
}
