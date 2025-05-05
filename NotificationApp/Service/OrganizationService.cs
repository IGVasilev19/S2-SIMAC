using BLL;
using DAL;
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
        OrganizationRepository organizationRepository = new OrganizationRepository();
        public IEnumerable<Organization> GetAll()
        {
           
            IEnumerable<Organization> OrganizationList = organizationRepository.GetAll();
            return OrganizationList;
        }
        public void Add(Organization organization)
        {
            organizationRepository.Add(organization);
        }
        public void Delete(Organization organization) 
        {
            organizationRepository.Delete(organization.OrganizationId);
        }
        public Organization GetById(int id) 
        {
            Organization organization = organizationRepository.GetById(id);
            return organization;
        }
    }
}
