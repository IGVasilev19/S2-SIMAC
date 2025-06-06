using Microsoft.AspNetCore.Mvc;
using NotificationApp.Models;
using System.Diagnostics;
using BLL;
using DAL;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Service.Interfaces;
using NotificationApp.Models.DTO_View_Models;
using Service;

namespace NotificationApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IOrganizationService _organizationService;
        private readonly IAccountService _accountService;

        public AdminController(IOrganizationService orgService, IAccountService accountService)
        {
            _organizationService = orgService;
            _accountService = accountService;
        }

        public IActionResult AdminPanel()
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (accountId != null)
            {
                if (int.TryParse(accountId, out int id))
                {
                    var adminAccount = _accountService.GetById(id);

                    AdminPanelViewModel vm = new();
                    vm.Organizations = new List<OrganizationViewModel>();

                    var allOrgs = _organizationService.GetAll();

                    foreach (var org in allOrgs)
                    {
                        var manager = _accountService.GetManagerByOrganizationId(org.OrganizationId);

                        OrganizationViewModel orgVm = new()
                        {
                            OrganizationId = org.OrganizationId,
                            OrganizationName = org.Name,
                            ManagerName = manager?.Name
                        };
                        vm.Organizations.Add(orgVm);
                    }
                    return View(vm);
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult SearchOrganizations(AdminPanelViewModel vm)
        {
            var accountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (accountId != null)
            {
                if (int.TryParse(accountId, out int id))
                {
                    var adminAccount = _accountService.GetById(id); // Creator's account object to get Org Id from

                    vm.Organizations = new List<OrganizationViewModel>();

                    var filteredOrgs = _organizationService.SearchOrganizations(vm.Search);

                    foreach (var org in filteredOrgs)
                    {
                        OrganizationViewModel orgVm = new()
                        {
                            OrganizationId = org.OrganizationId,
                            OrganizationName = org.Name,
                            //ManagerName = TODO: Fix this
                        };
                        vm.Organizations.Add(orgVm);
                    }
                    return View("AdminPanel", vm);
                }
            }
            return View("AdminPanel");
        }


        [HttpPost]
        public IActionResult CreateOrganizationWithManager(AdminCreatePanelViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("AdminCreatePanel");
            }

            var newOrganization = new Organization(model.OrganizationName);
            int newOrganizationId = _organizationService.AddOrganization(newOrganization);

            const int managerRoleId = 2; // RoleId 2 = "Manager"
            _accountService.SignUp(
                model.Name,
                model.Email,
                model.Password,
                newOrganizationId,
                managerRoleId
            );

            return RedirectToAction("AdminPanel", "Admin");
        }

        public IActionResult AdminCreatePanel()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AdminEditPanel(int organizationId)
        {
            var organization = _organizationService.GetById(organizationId);
            if (organization == null)
            {
                return NotFound();
            }

            var manager = _accountService.GetManagerByOrganizationId(organizationId);

            var vm = new AdminEditPanelViewModel
            {
                OrganizationId = organization.OrganizationId,
                OrganizationName = organization.Name,
                Name = manager.Name,
                Email = manager.Email,
                Password = manager.Password
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult SaveOrganization(AdminEditPanelViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("AdminEditPanel", model);
            }

            var organization = _organizationService.GetById(model.OrganizationId);
            if (organization == null)
            {
                return NotFound();
            }

            if (organization.Name != model.OrganizationName)
            {
                organization.Name = model.OrganizationName;
                _organizationService.Update(organization);
            }

            var manager = _accountService.GetManagerByOrganizationId(model.OrganizationId);

            if (manager != null)
            {
                var newPassword = string.IsNullOrWhiteSpace(model.Password) ? manager.Password : model.Password;

                if (manager.Name != model.Name || manager.Email != model.Email || newPassword != manager.Password)
                {
                    _accountService.Update(
                        manager.AccountId,
                        model.Name,
                        model.Email,
                        newPassword,
                        manager.OrganizationId,
                        manager.RoleId
                    );
                }
            }

            return RedirectToAction("AdminPanel", "Admin");
        }

        [HttpPost]
        public IActionResult DeleteOrganization(int organizationId)
        {
            _organizationService.DeleteById(organizationId);

            TempData["SuccessMessage"] = "The organization was successfully deleted.";
            return RedirectToAction("AdminPanel", "Admin");
        }
    }
}