namespace NotificationApp.Models
{
    public class RolesCreateEditPanelViewModel
    {
        public int? RoleId { get; set; }
        public string RoleNAme { get; set; }
        public List<PermissionViewModel> Permissions { get; set; }
        public List<PermissionViewModel> SelectedPermissions { get; set; }
        public string Action { get; set; }
    }
}
