namespace NotificationApp.Models
{
    public class AnalyticsViewModel
    {
        public List<DeviceViewModel> DevicePanels { get; set; }
        public List<(NotificationViewModel,int)> NotificationPanels { get; set; }
    }
}
