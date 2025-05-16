using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;

namespace DAL.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        public Account GetByEmail(string email);

        public Account GetByOrganization(int organizationId);
    }
}
