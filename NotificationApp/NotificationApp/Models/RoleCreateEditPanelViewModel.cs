using BLL;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace NotificationApp.Models
{
    public class RoleCreateEditPanelViewModel
    {
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Role name is required.")]
        [MaxLength(50)]
        public string RoleName { get; set; }
        public List<PermissionViewModel> Permissions { get; set; } = new();
        public List<PermissionViewModel> ChildPermissions { get; set; } = new();
        public List<PermissionViewModel> SelectedPermissions { get; set; } = new();
    }
}
