using BLL;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class RoleService : IRoleService
    {
        private readonly IRoleService _roleService;
        public RoleService(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public void AssignPermission(int roleId, IEnumerable<Permission> permissions)
        {
            _roleService.AssignPermission(roleId, permissions);
        }

        public IEnumerable<Role> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
