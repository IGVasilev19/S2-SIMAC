using BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IOrganizationService : IService<Organization>
    {
        IEnumerable<Organization> GetAll();
        void Add(Organization organization);
        void Delete(Organization organization);
        Organization GetById(int id);
    }
}
