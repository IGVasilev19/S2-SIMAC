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
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;

        public AccountController (IAccountService accountService, IRoleService roleService)
        {
            _accountService = accountService;
            _roleService = roleService;
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

        [HttpPost]
        public IActionResult SearchAccounts(AccountPanelViewModel vm) //TODO: Connect ;P
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                Account acc = _accountService.GetById(id);
                var accounts = _accountService.SearchAccounts(vm.Search, acc.OrganizationId);
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

                vm.Accounts = vmAccounts;

                return View("AccountPanel", vm);
            }
            else
            {
                return View("AccountPanel", vm);
            }
        }

        [HttpPost]
        public IActionResult DeleteAccount(int id)
        {
            _accountService.DeleteById(id);

            return RedirectToAction("AccountPanel");
        }

        // [Permission("Manager", "Admin")]
        public IActionResult AccountCreatePanel() //TODO: MINA Add safety check pls
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                Account creatorAccount = _accountService.GetById(id);
                List<Role> allRoles = (List<Role>)_roleService.GetAllRolesByOrganisationId(creatorAccount.OrganizationId);
                AccountCreatePanelViewModel vm = new();
                foreach(var role in allRoles)
                {
                    RoleViewModel rVM = new();
                    rVM.RoleId = role.RoleId;
                    rVM.Name = role.Name;
                    vm.Roles.Add(rVM);
                }
                vm.SelectedRole = new();

                return View(vm);
            }
            throw new NotImplementedException("TODO");
        }

        [HttpPost]
        public IActionResult CreateAccount(AccountCreatePanelViewModel accountVM, int SelectedRole)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.ErrorMessage = "Please fill in all required fields.";
                return View(accountVM); //TODO: MINA add validation it is an order
            }

            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                Account creator = _accountService.GetById(id);
                _accountService.SignUp(accountVM.Name, accountVM.Email, accountVM.Password, creator.OrganizationId, SelectedRole);
                return RedirectToAction("AccountPanel");
            }

            return View(accountVM);
        }

        public IActionResult AccountEditPanel(int selectedAccountId) //TODO: MINA Add safety check pls
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(accountId, out int id))
            {
                Account creatorAccount = _accountService.GetById(id); // Creator's account object to get Org Id from
                Account selectedAccount = _accountService.GetById(selectedAccountId); // Selected account object to edit

                AccountEditPanelViewModel vm = new();

                vm.Name = selectedAccount.Name;
                vm.Email = selectedAccount.Email;
                Role currentRole = _roleService.GetById(selectedAccount.RoleId);

                //vm.Role = new();
                //vm.Role.RoleId = currentRole.RoleId;
                //vm.Role.Name = currentRole.Name;


                List<Role> allRoles = (List<Role>)_roleService.GetAllRolesByOrganisationId(creatorAccount.OrganizationId); // Add all roles to the edit view for display purposes
                foreach (var role in allRoles)
                {
                    RoleViewModel rVM = new();
                    rVM.RoleId = role.RoleId;
                    rVM.Name = role.Name;
                    vm.Roles.Add(rVM);
                }

                Role roleOfSelectedAccount = _roleService.GetById(selectedAccount.RoleId); // Get the role of the selected account

                RoleViewModel roleVM = new();
                roleVM.RoleId = roleOfSelectedAccount.RoleId;
                roleVM.Name = roleOfSelectedAccount.Name;

                vm.SelectedRole = roleVM; // Set the selected role to the the selected account
                
                return View(vm);
            }

            return View();
        }

        [HttpPost]
        public IActionResult EditAccount(AccountEditPanelViewModel accountVM)
        {
            if (ModelState.IsValid == false)
            {
                ViewBag.ErrorMessage = "Please fill in all required fields.";
                return RedirectToAction("AccountEditPanel"); //TODO: MINA add validation it is an order
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