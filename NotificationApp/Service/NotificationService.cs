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
using Microsoft.Data.SqlClient;

namespace Service
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IDeviceService _deviceService;

        public NotificationService(INotificationRepository notificationRepository, IDeviceService deviceService)
        {
            _notificationRepository = notificationRepository;
            _deviceService = deviceService;
        }

        public void DeleteById(int id)
        {
            var notification = _notificationRepository.GetById(id);
            if (notification == null)
                throw new KeyNotFoundException("Notification not found.");

            _notificationRepository.Delete(id);
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

        public void MarkNotificationAsUnread(int notificationId, int accountId)
        {
            _notificationRepository.MarkAsUnread(notificationId, accountId);
        }

        public bool HasUserReadNotification(int accountId, int notificationId)
        {
            return _notificationRepository.IsRead(notificationId, accountId);
        }

        public IEnumerable<Notification> GetNotificationsForUser(Account account, List<int> permissionIds)
        {
            return _notificationRepository.GetNotificationsForUser(account.OrganizationId, permissionIds);
        }

        public IEnumerable<Notification> SearchNotifications(string filter, Account account, List<int> permissionIds, IEnumerable<Notification> notifications)
        {
            IEnumerable<Notification> filteredNotifications = notifications;
            if (!string.IsNullOrEmpty(filter))
            {
                filteredNotifications = filteredNotifications.Where(s => s.Title.ToUpper().Contains(filter.ToUpper()) || s.Content.ToUpper().Contains(filter.ToUpper()));
            }
            return filteredNotifications;
        }

        public IEnumerable<Notification> FilterNotificationsRead(IEnumerable<Notification> notifications, bool? read, Account account)
        {
            IEnumerable<Notification> filtered = new List<Notification>();
            switch (read)
            {
                case true:
                    foreach (Notification notification in notifications)
                    {
                        if (_notificationRepository.IsRead(notification.NotificationID, account.AccountId))
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
            }
            return filtered;
        }

        public IEnumerable<Notification> GetNotificationsOrderedByDate(Account account, List<int> permissionIds)
        {
            return _notificationRepository.GetNotificationsOrderedByDate(account.OrganizationId, permissionIds);
        }

        public void AddNotification(Notification notification) 
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification), "Notification cannot is null");
            }
            if (notification.DeviceId > 0)
            {
                int deviceId = Convert.ToInt32(notification.DeviceId);
                Device device = _deviceService.GetById(deviceId);

                // Add the requested line at the top of the content
                notification.Content = $"This notification links to {device.Name}\n\n{notification.Content}";
            }

            notification.Date = DateTime.UtcNow;
            _notificationRepository.Add(notification);
        }

        public List<Notification> GetNotificationsByOrganization(int organizationId)
        {
            return _notificationRepository.GetNotificationsByOrganization(organizationId);
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
