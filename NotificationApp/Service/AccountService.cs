using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using DAL;
using BCrypt.Net;

namespace Service
{
    public class AccountService
    {
        private readonly AccountRepository _accountRepository;
        public AccountService()
        {
            _accountRepository = new AccountRepository();
        }
        //DATABASE TESTING---------------------------------------------
        public void SignUp(string name, string email, string password, int roleId)
        {
            // Hash the password with a salt using BCrypt  
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Create a new account with the hashed password  
            Account newAccount = new Account(name, email, hashedPassword, roleId);

            // Add the account to the repository  
            _accountRepository.Add(newAccount);
        }
        //DATABASE TESTING---------------------------------------------

        public void SignUp(string name, string email, string password, Role role)
        {
            // Hash the password with a salt using BCrypt  
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // Create a new account with the hashed password  
            Account newAccount = new Account(name, email, hashedPassword, role);

            // Add the account to the repository  
            _accountRepository.Add(newAccount);
        }



        public Account LogIn(string email, string password)
        {
            Account account = _accountRepository.GetByEmail(email);
            if (account != null && BCrypt.Net.BCrypt.Verify(password, account.Password))
            {
<<<<<<< Updated upstream
                return account;
            }
            else
            {
                return null;
=======
                if (account.Email == email &&  BCrypt.Net.BCrypt.Verify(password, account.Password))
                {
                    return account;
                }
>>>>>>> Stashed changes
            }
        }
    }
}
