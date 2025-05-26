using NotificationApp.Models.DTO_View_Models;

namespace NotificationApp.Models
{
    public class AdminPanelViewModel
    {
        public string AdminRoleName { get; set; } = "Admin";
        public List<OrganizationViewModel> Organizations {  get; set; }

    }
}
