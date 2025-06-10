using BLL;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using NotificationApp.Models.DTO_View_Models;
using System.ComponentModel.DataAnnotations;

namespace NotificationApp.Models
{
    public class AccountCreatePanelViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;    

        [Required(ErrorMessage = "Email is required")]
        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(255)]
        public string Password { get; set; } = string.Empty;

        [ValidateNever]
        [BindNever]
        public List<RoleViewModel> Roles { get; set; } = new(); 

        [Required(ErrorMessage = "Role is required")]
        [ValidateNever]
        public RoleViewModel SelectedRole { get; set; } = new RoleViewModel();
    }
}
