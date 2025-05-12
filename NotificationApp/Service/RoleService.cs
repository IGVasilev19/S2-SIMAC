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

        public void Add(Role role)
        {
            throw new NotImplementedException();
        }

        public void AssignPermission(int roleId, IEnumerable<Permission> permissions)
        {
            _roleService.AssignPermission(roleId, permissions);
        }

        public void Delete(int roleId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Role> GetAll()
        {
            throw new NotImplementedException();
        }

        public Role GetById(int roleId)
        {
            throw new NotImplementedException();
        }

        public void RemovePermission(int roleId, Permission permission)
        {
            throw new NotImplementedException();
        }

        public void Update(Role role)
        {
            throw new NotImplementedException();
        }
    }
}
