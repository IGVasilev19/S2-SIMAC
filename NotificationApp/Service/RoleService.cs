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

        public int Add(Role role)
        {
            return _roleRepository.AddRole(role);
        }

        public void AssignPermission(int roleId, IEnumerable<Permission> permissions)
        {
            _roleRepository.AssignPermission(roleId, permissions);
        }

        public bool Delete(int roleId)
        {
            if(!_roleRepository.CheckIfRoleOccupied(roleId))
            {
                _roleRepository.Delete(roleId);
                return true;
            }
            return false;
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

        public IEnumerable<Role> GetAllRolesByOrganisationId(int organizationId)
        {
            return _roleRepository.GetAllRolesByOrganisationId(organizationId);
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Role> SearchRoles(string filter, int organizationId)
        {
            IEnumerable<Role> filteredRoles = _roleRepository.GetAllRolesByOrganisationId(organizationId);
            if (!string.IsNullOrEmpty(filter))
            {
                filteredRoles = filteredRoles.Where(s => s.Name.ToUpper().Contains(filter.ToUpper()));
            }
            return filteredRoles;
        }
    }
}
