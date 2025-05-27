using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;

namespace Service.Interfaces
{
    public interface IAccountService : IService<Account>
    {
        void SignUp(string name, string email, string password, int organization, int role);
        Account LogIn(string email, string password);
        Account GetById(int id);
        void Update(int id, string name, string email, string password, int organizationId, int roleId);
        Account GetByEmail(string email);
        public IEnumerable<Account> GetByOrganization(int organizationId);
        public IEnumerable<Account> SearchAccounts(string filter, int organizationId);
        public Account GetManagerByOrganization(int organizationId);
    }
}
