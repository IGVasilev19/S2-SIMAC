using BLL;
using Microsoft.AspNetCore.Mvc.Rendering;
using NotificationApp.Models.DTO_View_Models;

namespace NotificationApp.Models
{
    public class AccountCreateEditPanelViewModel
    {
        public string? Name {get; set;}
        public string? Email { get; set; }
        public string? Password {get; set;}
        public List<RoleViewModel>? Roles { get; set; } = new();
        public RoleViewModel? SelectedRole {get; set;}
    }
}
