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

                    List<Organization> allOrgs = (List<Organization>)_organizationService.GetAll();

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

        //[HttpPost]
        //public IActionResult CreateOrginization()
        //{                                                                                     

        //}
        
        public IActionResult AdminCreateEditPanel()
        {
            return View();
        }
    }
}