using System.Data;

namespace BLL
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Organization Organization { get; set; }
        public Role AccountRole { get; set; }
        public List<Notification> Notifications { get; set; }
        //DATABASE TESTING---------------------------------------------
        public int RoleId { get; set; }
        public int OrganizationId { get; set; }

        public Account(string name, string email, string password, int organization, int role) // For creating account (no id)
        {
            Name = name;
            Email = email;
            Password = password;
            OrganizationId = organization;
            RoleId = role;
            Notifications = new List<Notification>();   
        }

        public Account(int accountId, string name, string email, string password, int organization, int role) // For creating account (no id)
        {
            AccountId = accountId;
            Name = name;
            Email = email;
            Password = password;
            OrganizationId = organization;
            RoleId = role;
            Notifications = new List<Notification>();
        }
        //DATABASE TESTING---------------------------------------------

        public Account(string name, string email, string password, Organization organization, Role role) // For creating account (no id)
        {
            Name = name;
            Email = email;
            Password = password;
            Organization = organization;
            AccountRole = role;
            Notifications = new List<Notification>();
        }
        public Account(int accountId, string name, string email, Organization organization, Role role) // For displaying (no password)
        {
            AccountId = accountId;
            Name = name;
            Email = email;
            Organization = organization;
            AccountRole = role;
            Notifications = new List<Notification>();
        }
        public Account(int accountId, string name, string email, string password, Organization organization, Role role) // For logging in (all data)
        {
            AccountId = accountId;
            Name = name;
            Email = email;
            Password = password;
            Organization = organization;
            AccountRole = role;
            Notifications = new List<Notification>();
        }

        public Account(string password) 
        {
            Password = password;
        }
    }
}
