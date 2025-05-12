using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BCrypt.Net;
using Service.Interfaces;
using DAL.Interfaces;
using Service.Utility;

namespace Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public IEnumerable<Account> GetAll()
        {
            return _accountRepository.GetAll();
        }

        //public void SignUp(string name, string email, string password, Organization organization, Role role)
        //{
        //    // Hash the password with a salt using BCrypt 
        //    string hashedPassword = PasswordHasher.Hash(password);

        //    // Create a new account with the hashed password  
        //    Account newAccount = new Account(name, email, hashedPassword, organization, role);

        //    // Add the account to the repository  
        //    _accountRepository.Add(newAccount);
        //}

        public void SignUp(string name, string email, string password, int organization, int role)
        {
            // Hash the password with a salt using BCrypt  
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Create a new account with the hashed password  
            Account newAccount = new Account(name, email, hashedPassword, organization, role);

            // Add the account to the repository  
            _accountRepository.Add(newAccount);
        }


        public Account LogIn(string email, string password)
        {
            IEnumerable<Account> accounts = _accountRepository.GetAll();

            foreach (Account account in accounts)
            {
                if (account.Email == email &&  PasswordHasher.Verify(password, account.Password))
                {
                    return account;
                }
                else if (account.Email == email && !BCrypt.Net.BCrypt.Verify(password, account.Password))
                {
                    return new Account("Invalid password");
                }
            }

            return null;
        }

        public Account GetById(int id) => _accountRepository.GetById(id);
    }
}
