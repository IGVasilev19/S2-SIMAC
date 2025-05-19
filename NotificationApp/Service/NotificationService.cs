using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using Service.Interfaces;
using DAL.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Service
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Notification> GetAll()
        {
            return _notificationRepository.GetAll();
        }

        public Notification GetById(int id)
        {
            return _notificationRepository.GetById(id);
        }

        public List<Notification> GetByPermission(int permissionId)
        {
            List<Notification> listOfNotifications = _notificationRepository.GetByPermission(permissionId);
            if (listOfNotifications.IsNullOrEmpty())
            {
                return null;
            }
            return listOfNotifications;
        }

        public void MarkNotificationAsRead(int accountId, int notificationId)
        {
            _notificationRepository.MarkAsRead(notificationId, accountId);
        }

        public bool HasUserReadNotification(int accountId, int notificationId)
        {
            return _notificationRepository.IsRead(notificationId, accountId);
        }

        public List<Notification> GetNotificationsForUser(Account account, List<int> permissionIds)
        {
            return _notificationRepository.GetNotificationsForUser(account.OrganizationId, permissionIds);
        }
    }
}
