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
    public class SystemController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly INotificationService _notificationService;
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;
        private readonly IDeviceService _deviceService;

        public SystemController(IAccountService accountService, INotificationService notificationService, IRoleService roleService, IPermissionService permissionService, IDeviceService deviceService)
        {
            _accountService = accountService;
            _notificationService = notificationService;
            _roleService = roleService;
            _permissionService = permissionService;
            _deviceService = deviceService;
        }

        [Authorize]
        public IActionResult Inbox()
        {
            //foreach (Device device in _deviceService.GetAll())
            //{
            //    Console.WriteLine(device.ToString());
            //    Console.WriteLine("---------------------");
            //    Console.WriteLine($"Status:------------ {_deviceService.GetDeviceStatus(device)}");
            //    Console.WriteLine($"Device by ID:----------- {_deviceService.GetById(device.DeviceID).ToString()}");
            //}
            List<Device> devices = _deviceService.GetAll().ToList();
            devices[1].SetStatus(Status.ONLINE);
            _deviceService.Update(devices[1]);

            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (accountId != null)
            {
                if (int.TryParse(accountId, out int id))
                {
                    var account = _accountService.GetById(id);
                    var notifications = _notificationService.GetByPermission(account.RoleId); //TODO: This needs to use PermissionId note RoleId
                    var vmNotifications = new List<NotificationViewModel>();

                    foreach (var notification in notifications)
                    {
                        vmNotifications.Add(new NotificationViewModel
                        {
                            NotificationID = notification.NotificationID,
                            Title = notification.Title,
                            Content = notification.Content,
                            Important = notification.Important,
                            Date = notification.Date.ToString("yyyy-MM-dd HH:mm:ss")
                        });
                    }

                    //DATABASE TESTING---------------------------------------------
                    InboxViewModel vm = new InboxViewModel
                    {
                        AccountId = account.AccountId,
                        AccountName = account.Name,
                        AccountEmail = account.Email,
                        AccountPassword = account.Password,
                        AccountOrganization = account.OrganizationId.ToString(),
                        AccountRole = account.RoleId.ToString(),
                        Notifications = vmNotifications
                    };
                    //DATABASE TESTING---------------------------------------------

                    /*InboxViewModel vm = new InboxViewModel
                    {
                        AccountId = account.AccountId,
                        AccountName = account.Name,
                        AccountEmail = account.Email,
                        AccountPassword = account.Password,
                        AccountRole = account.AccountRole.Name,
                        Notifications = vmNotifications
                    };*/

                    return View(vm);
                }
                else
                {
                    return View();
                }
            }
                return View();
        }
        
        public IActionResult DevicesPanel()
        {
            return View();
        }
        
        public IActionResult DevicesCreateEditPanel()
        {
            return View();
        }

        public IActionResult AdminPanel()
        {
            return View();
        }
        
        public IActionResult AdminCreateEditPanel()
        {
            return View();
        }

        public IActionResult AccountPanel()
        {
            var accounts = _accountService.GetAll();
            List<AccountViewModel> vmAccounts = new();
            foreach (var account in accounts)
            {
                var role = _roleService.GetById(account.RoleId);
                var vmRole = new RoleViewModel
                {
                    RoleId = role.RoleId,
                    Name = role.Name
                };
                vmAccounts.Add(new AccountViewModel
                {
                    AccountId = account.AccountId,
                    Name = account.Name,
                    Email = account.Email,
                    Password = account.Password,
                    Role = vmRole
                });
            }

            var viewmodel = new AccountPanelViewModel
            {
                Accounts = vmAccounts
            };
            
            return View(viewmodel);
        }

        public IActionResult AccountCreateEditPanel()
        {
            return View();
        }
        public IActionResult Analytics()
        {
            return View();
        }

        public IActionResult RolesPanel()
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                Account account = _accountService.GetById(id);
                int orgId = account.OrganizationId;
                List<RoleViewModel> allRoles = new();
                foreach(Role role in _roleService.GetAllRolesByOrganisationId(orgId))
                {
                    allRoles.Add(new RoleViewModel
                    {
                        RoleId = role.RoleId,
                        Name = role.Name
                    });
                }
                RolesPanelViewModel vm = new RolesPanelViewModel
                {
                    Roles = allRoles
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
                List<Permission> allPermissions = (List<Permission>)_permissionService.GetAll();
                RolePanelViewModel vm = new();
                vm.SelectedPermissions = new List<PermissionViewModel>();
                foreach (var permission in allPermissions)
                {
                    PermissionViewModel pVM = new();
                    pVM.PermissionId = permission.PermissionId;
                    pVM.Name = permission.Name;
                    vm.Permissions.Add(pVM);
                }
                return View("RolesCreatePanel", vm);
            }
            throw new Exception("User Not Found");
        }

        //TODO: IMPLEMENT FRONT END
        [HttpPost]
        public IActionResult CreateRole(RolePanelViewModel vm)
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
                List<Permission> allPermissions = (List<Permission>)_permissionService.GetAll();
                List<Permission> selectedPermissions = (List<Permission>)_permissionService.GetPermissionsByRoleId(roleId);
                RolePanelViewModel vm = new();

                vm.Permissions = new();
                foreach (var permission in allPermissions)
                {
                    PermissionViewModel pVM = new();
                    pVM.PermissionId = permission.PermissionId;
                    pVM.Name = permission.Name;
                    vm.Permissions.Add(pVM);
                }

                vm.SelectedPermissions = new List<PermissionViewModel>();
                foreach(var permission in selectedPermissions)
                {
                    PermissionViewModel pVM = new();
                    pVM.PermissionId = permission.PermissionId;
                    pVM.Name = permission.Name;
                    vm.SelectedPermissions.Add(pVM);
                }
                return View("RolesEditPanel", vm);
            }
            throw new Exception("UserId Not Found");
        }

        [HttpPost]

        public IActionResult EditRole(RolePanelViewModel vm)
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                Account account = _accountService.GetById(id);
                Role role = new Role(id, vm.RoleName, account.OrganizationId);
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
            _roleService.Delete(roleId);
            return RedirectToAction("RolesPanel");
        }

        //TODO: DELETE THIS LATER
        public IActionResult RolesCreateEditPanel()
        {
            return View();
        }
    }
}