namespace NotificationApp.Models
{
    public class RolesPanelViewModel
    {
        public int? RoleId { get; set; }
        public List<PermissionViewModel> Permissions { get; set; }
        public List<PermissionViewModel> SelectedPermssions { get; set; }
    }
}
