using Azure.Identity;
using BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using NotificationApp.Models;
using NotificationApp.Models.DTO_View_Models;
using Service.Interfaces;
using System.Collections.Specialized;
using System.Security.Claims;

namespace NotificationApp.Controllers
{
    public class RoleController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;

        public RoleController (IAccountService accountService, IRoleService roleService, IPermissionService permissionService)
        {
            _accountService = accountService;
            _roleService = roleService;
            _permissionService = permissionService;
        }

        public IActionResult RolesPanel()
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                Account account = _accountService.GetById(id);
                int orgId = account.OrganizationId;
                RolesPanelViewModel vm = new();
                vm.Roles = new();

                foreach (Role role in _roleService.GetAllRolesByOrganisationId(orgId))
                {
                    var newRole = new RoleViewModel
                    {
                        RoleId = role.RoleId,
                        Name = role.Name
                    };

                    vm.Roles.Add(newRole);
                };

                return View(vm);
            }
            else
            {
                return View();
            }
        }


        //[HttpPost]

        //public IActionResult UpdateRole(int roleId, string roleName, IEnumerable<int> permissionIds)
        //{
        //    if (string.IsNullOrEmpty(roleName) || permissionIds == null)
        //    {
        //        return BadRequest("Invalid role name or permissions.");
        //    }

        //    Role role = _roleService.GetById(roleId);
        //    role.Name = roleName;
        //    _roleService.Update(role);

        //    //List<Permission> permissions = permissionIds
        //    //    .Select(permissionId => _permissionService.GetById(permissionId))
        //    //    .ToList();

        //    var permissions = permissionIds
        //    .Select(id => _permissionService.GetById(id))
        //    .Where(p => p != null);

        //    _roleService.AssignPermission(roleId, permissions);

        //    return RedirectToAction("RolesPanel");
        //}

        //TODO: IMPLEMENT FRONT END
        public IActionResult RolesCreatePanel()
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                RoleCreateEditPanelViewModel vm = new();

                List<List<Permission>> allPermissions = (List<List<Permission>>)_permissionService.GetParentAndChildPermissions();

                foreach (Permission parentPermission in allPermissions[0]) //Populate Permissions Property (Parents)
                {
                    PermissionViewModel parentViewModel = new();
                    parentViewModel.PermissionId = parentPermission.PermissionId;
                    parentViewModel.Name = parentPermission.Name;
                    vm.Permissions.Add(parentViewModel);
                }

                foreach (Permission childPermission in allPermissions[1]) //Populate ChildPermissions Property (Children)
                {
                    PermissionViewModel childViewModel = new();
                    childViewModel.PermissionId = childPermission.PermissionId;
                    childViewModel.Name = childPermission.Name;
                    childViewModel.ParentId = childPermission.ParentId;
                    vm.ChildPermissions.Add(childViewModel);
                }

                return View("RolesCreatePanel", vm);


                //List<Permission> allPermissions = (List<Permission>)_permissionService.GetAll();

                //RoleCreateEditPanelViewModel vm = new();
                //vm.SelectedPermissions = new List<PermissionViewModel>();
                //foreach (var permission in allPermissions)
                //{
                //    PermissionViewModel pVM = new();
                //    pVM.PermissionId = permission.PermissionId;
                //    pVM.Name = permission.Name;
                //    pVM.ParentId = permission.ParentId;
                //    vm.Permissions.Add(pVM);
                //}
                //return View("RolesCreatePanel", vm);
            }
            throw new Exception("User Not Found");
        }

        //TODO: IMPLEMENT FRONT END
        [HttpPost]
        public IActionResult CreateRole(RoleCreateEditPanelViewModel vm, List<int> permissionIds) //TODO: Permission displaying in front end
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                Account account = _accountService.GetById(id);
                Role newRole = new Role(vm.RoleName, account.OrganizationId);
                _roleService.Add(newRole);
                List<Permission> selectedPermissions = new();
                foreach (var vmSelectedPermission in vm.SelectedPermissions)
                {
                    Permission p = _permissionService.GetById(vmSelectedPermission.PermissionId);
                    selectedPermissions.Add(p);
                }
                _roleService.AssignPermission(newRole.RoleId, selectedPermissions);
                return RedirectToAction("RolesPanel");
            }
            throw new Exception("UserId Not Found");
        }

        //TODO: IMPLEMENT FRONT END

        public IActionResult RolesEditPanel(int roleId)
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                RoleCreateEditPanelViewModel vm = new();

                Role selectedRole = _roleService.GetById(roleId);
                vm.RoleId = selectedRole.RoleId;
                vm.RoleName = selectedRole.Name;

                List<List<Permission>> allPermissions = (List<List<Permission>>)_permissionService.GetParentAndChildPermissions();

                foreach (Permission parentPermission in allPermissions[0]) //Populate Permissions Property (Parents)
                {
                    PermissionViewModel parentViewModel = new();
                    parentViewModel.PermissionId = parentPermission.PermissionId;
                    parentViewModel.Name = parentPermission.Name;
                    vm.Permissions.Add(parentViewModel);
                }

                foreach (Permission childPermission in allPermissions[1]) //Populate ChildPermissions Property (Children)
                {
                    PermissionViewModel childViewModel = new();
                    childViewModel.PermissionId = childPermission.PermissionId;
                    childViewModel.Name = childPermission.Name;
                    childViewModel.ParentId = childPermission.ParentId;
                    vm.ChildPermissions.Add(childViewModel);
                }

                List<Permission> currentPermissions = (List<Permission>)_permissionService.GetPermissionsByRoleId(roleId);

                foreach (Permission permission in currentPermissions) //Populate SelectedPermissions Property 
                {
                    PermissionViewModel selectedPermission = new();
                    selectedPermission.PermissionId = permission.PermissionId;
                    selectedPermission.Name = permission.Name;
                    if (permission.ParentId != null)
                    {
                        selectedPermission.ParentId = permission.ParentId;
                    }
                    vm.SelectedPermissions.Add(selectedPermission);
                }
                return View(vm);
            }
            throw new Exception("TODO");
        }

        [HttpPost]
        public IActionResult EditRole(RoleCreateEditPanelViewModel vm) //TODO: Needs to be hooked up to Front-End
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                Account account = _accountService.GetById(id);

                Role role = new Role(vm.RoleId, vm.RoleName, account.OrganizationId);
                _roleService.Update(role);
                List<Permission> selectedPermissions = new();
                foreach (var vmSelectedPermission in vm.SelectedPermissions)
                {
                    Permission p = _permissionService.GetById(vmSelectedPermission.PermissionId);
                    selectedPermissions.Add(p);
                }
                _roleService.AssignPermission(role.RoleId, selectedPermissions);
                return RedirectToAction("RolesPanel");
            }
            throw new Exception("UserId Not Found");
        }

        [HttpPost]

        public IActionResult DeleteRole(int roleId)
        {
            if (_roleService.Delete(roleId))
            {
                return RedirectToAction("RolesPanel", "Role");
            }
            else
            {
                ViewBag.ErrorMessage = "This role is being used by 1 or more accounts and can't be deleted.";
                return RedirectToAction("RolesPanel", "Role");
            }
        }
    }
}