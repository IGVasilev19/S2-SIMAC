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
        //DATABASE TESTING---------------------------------------------
        public int RoleId { get; set; }
        public List<Notification> Notifications { get; set; }

        //DATABASE TESTING---------------------------------------------
        public Account(string name, string email, string password, int role) // For creating account (no id)
        {
            Name = name;
            Email = email;
            Password = password;
            RoleId = role;
            Notifications = new List<Notification>();   
        }

        public Account(int accountId, string name, string email, string password, int role) // For creating account (no id)
        {
            AccountId = accountId;
            Name = name;
            Email = email;
            Password = password;
            RoleId = role;
            Notifications = new List<Notification>();
        }
        //DATABASE TESTING---------------------------------------------

        public Account(string name, string email, string password, Role role) // For creating account (no id)
        {
            Name = name;
            Email = email;
            Password = password;
            AccountRole = role;
            Notifications = new List<Notification>();
        }
        public Account(int accountId, string name, string email, Role role) // For displaying (no password)
        {
            AccountId = accountId;
            Name = name;
            Email = email;
            AccountRole = role;
            Notifications = new List<Notification>();
        }
        public Account(int accountId, string name, string email, string password, Role role) // For logging in (all data)
        {
            AccountId = accountId;
            Name = name;
            Email = email;
            AccountRole = role;
            Notifications = new List<Notification>();
        }

        public Account(string password) 
        {
            Password = password;
        }
    }
}
