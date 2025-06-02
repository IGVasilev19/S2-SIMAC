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

        public OrganizationService(IOrganizationRepository organizationRepository)
        {
            _organizationRepository = organizationRepository;
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
                throw new Exception("Organization with this name already exists."); //TODO: Handle this exception  
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

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
