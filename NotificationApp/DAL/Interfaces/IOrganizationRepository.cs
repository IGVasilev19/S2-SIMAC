using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        bool NameExists(string name);
        int AddOrganization(Organization organization);
    }
}
