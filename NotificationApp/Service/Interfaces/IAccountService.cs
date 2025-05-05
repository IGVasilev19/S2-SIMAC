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
        //DATABASE TESTING---------------------------------------------
        void SignUp(string name, string email, string password, int organization, int role);
        //DATABASE TESTING---------------------------------------------
        void SignUp(string name, string email, string password, Organization organization, Role role);
        Account LogIn(string email, string password);
        Account GetById(int id);
    }
}
