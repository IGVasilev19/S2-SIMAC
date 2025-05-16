using BLL;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NotificationApp.Models
{
    public class RoleCreateEditPanelViewModel
    {
        public string? RoleName { get; set; }
        public List<PermissionViewModel> Permissions { get; set; } = new();
        public List<PermissionViewModel> SelectedPermissions { get; set; } = new();
    }
}
