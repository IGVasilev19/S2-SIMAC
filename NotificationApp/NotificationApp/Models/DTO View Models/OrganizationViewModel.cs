namespace NotificationApp.Models.DTO_View_Models
{
    public class OrganizationViewModel
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string? ManagerName { get; set; } // This is used for the AdminPanel view to show pairings of Org + Manager
    }
}
