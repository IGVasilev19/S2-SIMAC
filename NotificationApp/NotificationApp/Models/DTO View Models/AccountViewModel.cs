using BLL;
using NotificationApp.Models.DTO_View_Models;
using System.ComponentModel.DataAnnotations;

namespace NotificationApp.Models
{
    public class AccountViewModel
    {
        public int? AccountId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password {  get; set; }
        public RoleViewModel Role { get; set; }
    }
}
