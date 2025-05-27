using Azure.Identity;
using BLL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        [Permission("AccountManagement")]
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

        [HttpPost]
        public IActionResult SearchRoles(RolesPanelViewModel vm) //TODO: Connect??????????? ;(
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                Account account = _accountService.GetById(id);
                int orgId = account.OrganizationId;
                vm.Roles = new();

                foreach (Role role in _roleService.SearchRoles(vm.Search, orgId))
                {
                    var newRole = new RoleViewModel
                    {
                        RoleId = role.RoleId,
                        Name = role.Name
                    };

                    vm.Roles.Add(newRole);
                };

                return View("RolesPanel", vm);
            }
            else
            {
                return View("RolesPanel");
            }
        }


        
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
            }
            throw new Exception("User Not Found");
        }

        [HttpPost]
        public IActionResult CreateRole(RoleCreateEditPanelViewModel vm, List<int> permissionIds) 
        {
            //ModelState.Remove(nameof(RoleCreateEditPanelViewModel.RoleId));
            if (!ModelState.IsValid)
            {
                return RedirectToAction("RolesCreatePanel"); //TODO:
            }

            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                Account account = _accountService.GetById(id);
                Role newRole = new Role(vm.RoleName, account.OrganizationId);
                int idOfNewRole = _roleService.Add(newRole); // Make this return the new role object
                List<Permission> selectedPermissions = new();
                foreach (var pId in permissionIds)
                {
                    Permission p = _permissionService.GetById(pId);
                    selectedPermissions.Add(p);
                }
                _roleService.AssignPermission(idOfNewRole, selectedPermissions);
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
        public IActionResult EditRole(RoleCreateEditPanelViewModel vm, List<int> permissionIds) //TODO: Needs to be hooked up to Front-End
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("RolesEditPanel", new { roleId = vm.RoleId });
            }

            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                Account account = _accountService.GetById(id);

                Role role = new Role(vm.RoleId, vm.RoleName, account.OrganizationId);
                _roleService.Update(role);
                List<Permission> selectedPermissions = new();
                foreach (var pId in permissionIds)
                {
                    Permission p = _permissionService.GetById(pId);
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
            try
            {
                _roleService.Delete(roleId);

                return RedirectToAction("RolesPanel", "Role");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "This role is being used by 1 or more accounts and can't be deleted.";
                return RedirectToAction("RolesPanel", "Role");
            }
        }
    }
}