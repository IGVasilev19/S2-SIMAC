using BLL;
using DAL.Interfaces;
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
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public void Add(Role role)
        {
            _roleRepository.Add(role);
        }

        public void AssignPermission(Role role, IEnumerable<Permission> permissions)
        {
            _roleRepository.AssignPermission(role, permissions);
        }

        public void Delete(int roleId)
        {
            _roleRepository.Delete(roleId);
        }

        public IEnumerable<Role> GetAll()
        {
            return _roleRepository.GetAll();
        }

        public Role GetById(int roleId)
        {
            return _roleRepository.GetById(roleId);
        }
        public void Update(Role role)
        {
            _roleRepository.Update(role);
        }
    }
}
