using Microsoft.AspNetCore.SignalR;

namespace NotificationApp.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task JoinOrganization(string organizationId)
        {
            Console.WriteLine($"Joining group: {organizationId}");
            await Groups.AddToGroupAsync(Context.ConnectionId, organizationId);
        }


        public async Task LeaveOrganization(string organizationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, organizationId);
        }
    }
}
