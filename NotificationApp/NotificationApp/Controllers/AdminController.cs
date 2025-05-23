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

namespace NotificationApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly IOrganizationService _organizationService;

        public AdminController(IOrganizationService orgService)
        {
            _organizationService = orgService;
        }

       public IActionResult AdminPanel()
        {
            var organizations = _organizationService.GetAll();
            List<OrganizationViewModel> vmOrganizations = new();

            foreach (var org in organizations)
            {
                var vmOrg = new OrganizationViewModel
                {
                    OrganizationId = org.OrganizationId,
                    OrganizationName = org.Name
                };

                vmOrganizations.Add(vmOrg);
            }

            var viewmodel = new AdminPanelViewModel
            {
                Organizations = vmOrganizations
            };

            return View(viewmodel);
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