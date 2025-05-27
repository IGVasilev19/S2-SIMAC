using NotificationApp.Models.DTO_View_Models;

namespace NotificationApp.Models
{
    public class RolesPanelViewModel
    {
        public List<RoleViewModel> Roles { get; set; }
        public string? Search { get; set; }
    }
}
