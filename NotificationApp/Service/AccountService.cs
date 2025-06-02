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
            Account account = _accountRepository.GetByEmail(email);
            if (account != null && PasswordHasher.Verify(password, account.Password))
            {
                return account;
            }
            return null;
        }

        public Account GetById(int id) => _accountRepository.GetById(id);

        public void DeleteById(int id) => _accountRepository.Delete(id);

        public void Update(int id, string name, string email, string password, int organizationId, int roleId)
        {
            // Hash the password with a salt using BCrypt  
            string hashedPassword = PasswordHasher.Hash(password);

            // Create a new account with the hashed password  
            Account updatedAccount = new Account(id, name, email, hashedPassword, organizationId, roleId);

            // Update the account in the repository  
            _accountRepository.Update(updatedAccount);
        }
        public Account GetByEmail(string email)
        {
            return _accountRepository.GetByEmail(email);
        }

        public IEnumerable<Account> GetByOrganization(int organizationId)
        {
            return _accountRepository.GetByOrganization(organizationId);
        }

        public IEnumerable<Account> SearchAccounts(string filter, int organizationId)
        {
            IEnumerable<Account> filteredAccounts = _accountRepository.GetByOrganization(organizationId);
            if (!string.IsNullOrEmpty(filter))
            {
                filteredAccounts = filteredAccounts.Where(s => s.Name.ToUpper().Contains(filter.ToUpper()));
            }
            return filteredAccounts;
        }

        public Account GetManagerByOrganization(int organizationId)
        {
            if (organizationId <= 0)
            {
                throw new ArgumentNullException("Organization ID must be greater than zero."); // TODO: This is not handled 
            }
            return _accountRepository.GetManagerByOrganization(organizationId);
        }

        public IEnumerable<Account> GetByRoleId(int roleId)
        {
            if (roleId <= 0)
            {
                throw new ArgumentNullException("Role ID must be greater than zero."); // TODO: This is not handled 
            }
            return _accountRepository.GetByRoleId(roleId);
        }
    }
}
