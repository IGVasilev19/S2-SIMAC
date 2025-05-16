using System.Data;

namespace BLL
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int OrganizationId { get; set; }
        public int RoleId { get; set; }

        public Account(string name, string email, string password, int organizationId, int roleId) // For creating account (no id)
        {
            Name = name;
            Email = email;
            Password = password;
            OrganizationId = organizationId;
            RoleId = roleId;
        }

        public Account(int accountId, string name, string email, string password, int organizationId, int roleId) // For logging in (all data)
        {
            AccountId = accountId;
            Name = name;
            Email = email;
            Password = password;
            OrganizationId = organizationId;
            RoleId = roleId;
        }

        public Account(string password) 
        {
            Password = password;
        }
    }
}
