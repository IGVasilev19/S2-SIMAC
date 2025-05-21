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

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
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

        public IEnumerable<IEnumerable<Permission>> GetParentAndChildPermissions()
        {
            List<Permission> allPermissions = (List<Permission>)_permissionRepository.GetAll();

            List<Permission> parentPermissions = new();
            foreach (Permission permission in allPermissions)
            {
                if(permission.ParentId == null)
                {
                    parentPermissions.Add(permission);
                }
            }

            List<Permission> childPermissions = new();
            foreach (Permission permission in allPermissions)
            {
                if(permission.ParentId != null)
                {
                    childPermissions.Add(permission);
                }
            }

            List<List<Permission>> compressedPermissionsList = new();
            compressedPermissionsList.Add(parentPermissions);
            compressedPermissionsList.Add(childPermissions);

            return compressedPermissionsList;
        }
    }
}
