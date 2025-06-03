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
                    var adminAccount = _accountService.GetById(id); // Creator's account object to get Org Id from

                    AdminPanelViewModel vm = new();
                    vm.Organizations = new List<OrganizationViewModel>();

                    var allOrgs = _organizationService.GetAll();

                    foreach (var org in allOrgs)
                    {
                        OrganizationViewModel orgVm = new()
                        {
                            OrganizationId = org.OrganizationId,
                            OrganizationName = org.Name,
                            //ManagerName = TODO: Fix this
                        };
                        vm.Organizations.Add(orgVm);
                    }
                    return View(vm);
                }
            }
            return View();
        }

        [HttpPost]
        public IActionResult SearchOrganizations(AdminPanelViewModel vm) //TODO: Connect search to front-end
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
                return View("AdminCreatePanel"); // Return to the form if validation fails
            }

            var newOrganization = new Organization(model.OrganizationName);
            int newOrganizationId = _organizationService.AddOrganization(newOrganization);

            const int managerRoleId = 2; // Assuming "2" is the RoleId for "Manager"
            _accountService.SignUp(
                model.Name,
                model.Email,
                model.Password,
                newOrganizationId,
                managerRoleId
            );

            // Step 3: Redirect back to the Admin Panel
            return RedirectToAction("AdminPanel", "Admin");
        }
        
        public IActionResult AdminCreatePanel()
        {
            return View();
        }

        public IActionResult AdminEditPanel()
        {
            return View();
        }
    }
}