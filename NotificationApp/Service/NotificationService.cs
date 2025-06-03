using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using Service.Interfaces;
using DAL.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

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

        public IEnumerable<Notification> SearchNotifications(string  filter, Account account, List<int> permissionIds)
        {
            IEnumerable<Notification> filteredNotifications = _notificationRepository.GetNotificationsForUser(account.OrganizationId, permissionIds);
            if (!string.IsNullOrEmpty(filter))
            {
                filteredNotifications = filteredNotifications.Where(s => s.Title.ToUpper().Contains(filter.ToUpper()) || s.Content.ToUpper().Contains(filter.ToUpper()));
            }
            return filteredNotifications;
        }

        public IEnumerable<Notification> FilterNotifications(Account account, IEnumerable<Notification> notifications, bool? read, bool? important)
        {
            IEnumerable<Notification> filtered = new List<Notification> ();
            switch (read)
            {
                case true:
                    foreach (Notification notification in notifications)
                    {
                        if(_notificationRepository.IsRead(notification.NotificationID, account.AccountId))
                        {
                            filtered.Append(notification);
                        }
                    }; break;

                case false:
                    foreach (Notification notification in notifications)
                    {
                        if (!_notificationRepository.IsRead(notification.NotificationID, account.AccountId))
                        {
                            filtered.Append(notification);
                        }
                    }; break;

                default: filtered = notifications; break;
            }
            
            switch(important)
            {
                case true:
                    filtered = filtered.Where(f => f.Important); return filtered;
                case false:
                    filtered = filtered.Where(f => !f.Important); return filtered;
                default: return filtered;
            }
        }
        public void AddNotification(Notification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification), "Notification cannot be null");
            }
            notification.Date = DateTime.UtcNow;
            _notificationRepository.Add(notification);
        }

        public void BuildDeviceStatusNotification(Device device)
        {
            Notification notification = new Notification
            {
                DeviceId = device.DeviceID,
                Title = $"{device.Name} Status Update",
                Content = $"{device.Name} is now {device.DeviceStatus}.",
                Important = false,
                OrganizationId = device.OrganizationID,
                PermissionId = 3, //HARDCODED to Permissions
                Date = DateTime.UtcNow
            };
            AddNotification(notification);
        }
    }
}
