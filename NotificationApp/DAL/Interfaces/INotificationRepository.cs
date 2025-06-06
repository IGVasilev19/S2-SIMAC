using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BLL;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DAL.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        public List<Notification> GetByPermission(int permissionId);
        public void MarkAsRead(int notificationId, int accountId);
        public void MarkAsUnread(int notificationId, int accountId);
        public bool IsRead(int notificationId, int accountId);
        public List<Notification> GetNotificationsForUser(int organizationId, List<int> permissionIds);
        public List<Notification> GetNotificationsByOrganization(int organizationId);
    }
}
