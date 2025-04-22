using System.Data;

namespace BLL
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Role AccountRole { get; set; }

        public Account(string name, string email, string password, Role role) // For creating account (no id)
        {
            Name = name;
            Email = email;
            Password = password;
            AccountRole = role;
        }
        public Account(int accountId, string name, string email, Role role) // For displaying (no password)
        {
            AccountId = accountId;
            Name = name;
            Email = email;
            AccountRole = role;
        }
        public Account(int accountId, string name, string email, string password, Role role) // For logging in (all data)
        {
            AccountId = accountId;
            Name = name;
            Email = email;
            AccountRole = role;
        }
    }
}
