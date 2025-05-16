using BLL;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NotificationApp.Models
{
    public class RolePanelViewModel
    {
        public string? RoleName { get; set; }
        public List<PermissionViewModel> Permissions { get; set; }
        public List<PermissionViewModel> SelectedPermissions { get; set; }
    }
}
