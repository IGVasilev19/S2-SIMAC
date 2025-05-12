using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;

namespace Service.Interfaces
{
    public interface IRoleService : IService<Role>
    {
        void AssignPermission(int roleId, IEnumerable<Permission> permissions);
    }
}
