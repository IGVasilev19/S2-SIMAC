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
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public IEnumerable<Permission> GetAll()
        {
            return _permissionRepository.GetAll();
        }

        public Permission GetById(int id)
        {
            return _permissionRepository.GetById(id);
        }
        public IEnumerable<Permission> GetPermissionsByRoleId(int roleId)
        {
            return _permissionRepository.GetPermissionsByRoleId(roleId);
        }
    }
}
