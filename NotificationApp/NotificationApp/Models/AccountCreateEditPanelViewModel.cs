using BLL;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using NotificationApp.Models.DTO_View_Models;
using System.ComponentModel.DataAnnotations;

namespace NotificationApp.Models
{
    public class AccountCreateEditPanelViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name {get; set;}
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password {get; set;}
        public List<RoleViewModel> Roles { get; set; } = new();
        public RoleViewModel SelectedRole {get; set;}
    }
}
