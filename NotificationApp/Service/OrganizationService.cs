using BLL;
using DAL;
using DAL.Interfaces;
using Microsoft.Identity.Client;
using Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class OrganizationService : IOrganizationService
    {
        IOrganizationRepository _organizationRepository;
        IAccountService _accountService;
        IDeviceService _deviceService;
        IRoleService _roleService;
        INotificationService _notificationService;

        public OrganizationService(IOrganizationRepository organizationRepository, IAccountService accountService, IDeviceService deviceService, IRoleService roleService, INotificationService notificationService)
        {
            _organizationRepository = organizationRepository;
            _accountService = accountService;
            _deviceService = deviceService;
            _roleService = roleService;
            _notificationService = notificationService;
        }

        public IEnumerable<Organization> GetAll()
        {
            IEnumerable<Organization> OrganizationList = _organizationRepository.GetAll();
            return OrganizationList;
        }

        public void Add(Organization organization)
        {
            if (!_organizationRepository.NameExists(organization.Name))
            {
                throw new Exception("Organization with this name already exists.");
            }
            _organizationRepository.Add(organization);
        }

        public int AddOrganization(Organization organization)
        {
            return _organizationRepository.AddOrganization(organization);
        }

        public void Delete(Organization organization)
        {
            _organizationRepository.Delete(organization.OrganizationId);
        }

        public Organization GetById(int id)
        {
            Organization organization = _organizationRepository.GetById(id);
            return organization;
        }

        public void DeleteById(int organizationId)
        {
            var organization = _organizationRepository.GetById(organizationId);
            if (organization == null)
                throw new KeyNotFoundException("Organization not found.");

            // Step 1: Delete notifications
            var notifications = _notificationService.GetAll().Where(n => n.OrganizationId == organizationId);
            foreach (var notification in notifications)
            {
                _notificationService.DeleteById(notification.NotificationID);
            }

            // Step 2: Delete devices
            var devices = _deviceService.GetByOrganization(organizationId);
            foreach (var device in devices)
            {
                _deviceService.DeleteById(device.DeviceID);
            }

            // Step 3: Delete accounts
            var accounts = _accountService.GetByOrganization(organizationId);
            foreach (var account in accounts)
            {
                _accountService.DeleteById(account.AccountId);
            }

            // Step 4: Delete roles
            var roles = _roleService.GetAllRolesByOrganisationId(organizationId);
            foreach (var role in roles)
            {
                _roleService.Delete(role.RoleId);
            }

            // Step 5: Delete organization
            _organizationRepository.Delete(organizationId);
        }


        public void Update(Organization organization)
        {
            if (organization == null || organization.OrganizationId <= 0)
            {
                throw new ArgumentException("Invalid organization data.");
            }

            var existingOrg = _organizationRepository.GetById(organization.OrganizationId);

            if (existingOrg != null)
            {
                existingOrg.Name = organization.Name;
                _organizationRepository.Update(existingOrg);
            }
            else
            {
                throw new KeyNotFoundException("Organization not found.");
            }
        }

        public IEnumerable<Organization> SearchOrganizations(string filter)
        {
            IEnumerable<Organization> filteredOrganizations = _organizationRepository.GetAll();
            if(filter != null)
            {
                filteredOrganizations = filteredOrganizations.Where(s => s.Name.ToUpper().Contains(filter.ToUpper()));
            }
            return filteredOrganizations;
        }
    }
}
