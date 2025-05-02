using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;

namespace Service
{
    public interface IAccountService : IService<Account>
    {
        void SignUp(string name, string email, string password, Role role);
        Account LogIn(string email, string password);
    }


}
