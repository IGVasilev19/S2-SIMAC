using BLL;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NotificationApp.Models
{
    public class AccountCreateEditPanelViewModel
    {
        public string? username {get; set;}
        public string? password {get; set;}
        public List<Role>? roles {get; set;}
        public Role? selectedRole {get; set;}
    }
}
