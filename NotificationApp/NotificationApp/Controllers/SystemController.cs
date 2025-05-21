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
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (accountId != null)
            {
                if (int.TryParse(accountId, out int id))
                {
                    var account = _accountService.GetById(id);
                    //OLD//var notifications = _notificationService.GetByPermission(account.RoleId); //shows hardcoded notifications

                    var permissions = _permissionService.GetPermissionsByRoleId(account.RoleId);
                    var permissionIds = permissions.Select(p => p.PermissionId).ToList();
                    var notifications = _notificationService.GetNotificationsForUser(account, permissionIds);
                    

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

        [HttpPost]
        public IActionResult FilterNotifications(InboxViewModel vm) //TODO: Connect? Idk what I'm doing! Will this work? Will it not? Ig we'll find out!
        {
            if (!ModelState.IsValid)
            {
                return View("Inbox", vm);
            }

            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (accountId != null)
            {
                if (int.TryParse(accountId, out int id))
                {
                    var account = _accountService.GetById(id);

                    var permissions = _permissionService.GetPermissionsByRoleId(account.RoleId);
                    var permissionIds = permissions.Select(p => p.PermissionId).ToList();
                    var notifications = _notificationService.SearchNotifications(vm.Search, account, permissionIds);

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
                    vm.Notifications = vmNotifications;
                }
            }

            return View("Inbox", vm);
        }

        public IActionResult DevicesPanel()
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                Account account = _accountService.GetById(id);
                IEnumerable<Device> DevicesByOrganization = _deviceService.GetByOrganization(account.OrganizationId);
                List<DeviceViewModel> vmDevices = new();
                DevicePanelViewModel viewmodel = new DevicePanelViewModel();
                viewmodel.Devices = new();
                foreach (Device device in DevicesByOrganization)
                {
                    DeviceViewModel vm = new DeviceViewModel
                    {
                        DeviceID = device.DeviceID,
                        Name = device.Name,
                        Location = device.Location,
                        OrganizationID = device.OrganizationID,
                        DeviceStatus = device.DeviceStatus
                    };

                    viewmodel.Devices.Add(vm);
                }

                return View(viewmodel);
            }
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
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                var accounts = _accountService.GetAll();
                List<AccountViewModel> vmAccounts = new();
                
                foreach (var account in accounts)
                {
                    if (account.AccountId != id)
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
                }

                var viewmodel = new AccountPanelViewModel
                {
                    Accounts = vmAccounts
                };
                
                return View(viewmodel);
            }
            else
            {
                return View();
            }
        }

        public IActionResult AccountEditPanel()
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
            _roleService.Delete(roleId);
            return RedirectToAction("RolesPanel", "System");
        }

        [HttpPost]
        public IActionResult DeleteAccount(int id)
        {
            _accountService.DeleteById(id);

            return RedirectToAction("AccountPanel", "System");
        }

        public IActionResult AccountCreatePanel() //TODO: MINA Add safety check pls
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                Account creatorAccount = _accountService.GetById(id);
                List<Role> allRoles = (List<Role>)_roleService.GetAllRolesByOrganisationId(creatorAccount.OrganizationId);
                AccountCreateEditPanelViewModel vm = new();
                foreach(var role in allRoles)
                {
                    RoleViewModel rVM = new();
                    rVM.RoleId = role.RoleId;
                    rVM.Name = role.Name;
                    vm.Roles.Add(rVM);
                }
                return View("AccountCreatePanel", vm);
            }
            throw new NotImplementedException("TODO");
        }

        [HttpPost]
        public IActionResult CreateAccount(AccountCreateEditPanelViewModel accountVM)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.ErrorMessage = "Please fill in all required fields.";
                return View("AccountCreatePanel", accountVM); //TODO: MINA add validation it is an order
            }

            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                Account creator = _accountService.GetById(id);
                _accountService.SignUp(accountVM.Name, accountVM.Email, accountVM.Password, creator.OrganizationId, accountVM.SelectedRole.RoleId);
                return RedirectToAction("AccountPanel");
            }
            throw new NotImplementedException("TODO");
        }

        public IActionResult AccountEditPanel(AccountViewModel accountVM) //TODO: MINA Add safety check pls
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                AccountCreateEditPanelViewModel vm = new();

                Account creatorAccount = _accountService.GetById(id);

                List<Role> allRoles = (List<Role>)_roleService.GetAllRolesByOrganisationId(creatorAccount.OrganizationId);
                foreach (var role in allRoles)
                {
                    RoleViewModel rVM = new();
                    rVM.RoleId = role.RoleId;
                    rVM.Name = role.Name;
                    vm.Roles.Add(rVM);
                }

                vm.Name = accountVM.Name;
                vm.Email = accountVM.Email;
                vm.SelectedRole = new RoleViewModel();
                vm.SelectedRole.RoleId = accountVM.Role.RoleId;
                vm.SelectedRole.Name = accountVM.Role.Name;

                return View("AccountEditPanel", vm);
            }
            throw new NotImplementedException("TODO");
        }

        [HttpPost]
        public IActionResult EditAccount(AccountCreateEditPanelViewModel accountVM)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.ErrorMessage = "Please fill in all required fields.";
                return View("AccountEditPanel", accountVM); //TODO: MINA add validation it is an order
            }

            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                Account creator = _accountService.GetById(id);

                Account selectedAccount = _accountService.GetByEmail(accountVM.Email);

                if (string.IsNullOrEmpty(selectedAccount.Password))
                {
                    _accountService.Update(selectedAccount.AccountId, accountVM.Name, accountVM.Email, selectedAccount.Password, creator.OrganizationId, accountVM.SelectedRole.RoleId);
                    return RedirectToAction("AccountPanel");
                }
                else
                {
                    _accountService.Update(selectedAccount.AccountId, accountVM.Name, accountVM.Email, accountVM.Password, creator.OrganizationId, accountVM.SelectedRole.RoleId);
                    return RedirectToAction("AccountPanel");
                }
            }
            throw new NotImplementedException("TODO");
        }
    }
}