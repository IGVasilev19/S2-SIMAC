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
        public void MarkNotificationAsUnread(int notificationId, int accountId);
        public bool HasUserReadNotification(int accountId, int notificationId);
        public IEnumerable<Notification> GetNotificationsForUser(Account account, List<int> permissionIds);
        public List<Notification> GetNotificationsByOrganization(int organizationId);
        public IEnumerable<Notification> SearchNotifications(string filter, Account account, List<int> permissionIds, IEnumerable<Notification> notifications);
        public IEnumerable<Notification> FilterNotificationsRead(IEnumerable<Notification> notifications, bool? read, Account account);
        public IEnumerable<Notification> GetNotificationsOrderedByDate(Account account, List<int> permissionIds);
        public void AddNotification(Notification notification);
        public void BuildDeviceStatusNotification(Device device);
    }
}
