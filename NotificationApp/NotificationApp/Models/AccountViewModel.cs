using BLL;
using System.ComponentModel.DataAnnotations;

namespace NotificationApp.Models
{
    public class AccountViewModel
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password {  get; set; }
        public string Role { get; set; }
    }
}
