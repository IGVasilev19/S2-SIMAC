using BLL;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using NotificationApp.Models.DTO_View_Models;
using System.ComponentModel.DataAnnotations;

namespace NotificationApp.Models
{
    public class AccountEditPanelViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        public string? Password { get; set; }
        [ValidateNever]
        [BindNever]
        public List<RoleViewModel> Roles { get; set; } = new();
        [Required(ErrorMessage = "Role is required")]
        [ValidateNever]
        public RoleViewModel SelectedRole { get; set; }
    }
}
