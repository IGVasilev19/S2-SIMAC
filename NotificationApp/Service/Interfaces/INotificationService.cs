using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;

namespace Service.Interfaces
{
    public interface INotificationService : IService<Notification>
    {
        Notification GetById(int id);
        List<Notification> GetByPermission(int permissionId);
        public void MarkNotificationAsRead(int accountId, int notificationId);
        public bool HasUserReadNotification(int accountId, int notificationId);
        public List<Notification> GetNotificationsForUser(Account account, List<int> permissionIds);
        public IEnumerable<Notification> SearchNotifications(string filter, Account account, List<int> permissionIds);
    }
}
