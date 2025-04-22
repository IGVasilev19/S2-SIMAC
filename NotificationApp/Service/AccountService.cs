using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using DAL;

namespace Service
{
    public class AccountService
    {
        private readonly AccountRepository _accountRepository;
        public AccountService()
        {
            _accountRepository = new AccountRepository();
        }

        public void SignUp(string name, string email, string password, Role role)
        {
            Account newAccount = new Account(name, email, password, role);
            _accountRepository.Add(newAccount);
        }
    }
}
