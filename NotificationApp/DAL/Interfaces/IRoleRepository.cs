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
        void AssignPermissions(Role role, IEnumerable<Permission> permissions);
    }
}
